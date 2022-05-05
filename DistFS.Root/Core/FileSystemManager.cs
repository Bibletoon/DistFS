using System.Text.RegularExpressions;
using DistFS.Core.Interfaces;
using DistFS.Infrastructure;
using DistFS.Infrastructure.Database;
using DistFS.Models;
using DistFS.Nodes.Clients.Interfaces;
using DistFS.Tools;

namespace DistFS.Core;

public class FileSystemManager : IFileSystemManager
{
    private readonly IFileRepository _repository;
    private readonly INodeContext _nodeContext;
    private readonly INodeFileClient _nodeFileClient;
    private readonly IFileInfoContext _fileInfoContext;

    public FileSystemManager(IFileRepository repository, INodeContext nodeInfoManager, INodeFileClient nodeFileClient, IFileInfoContext fileInfoContext)
    {
        _repository = repository;
        _nodeContext = nodeInfoManager;
        _nodeFileClient = nodeFileClient;
        _fileInfoContext = fileInfoContext;
    }

    public async Task WriteFileAsync(string localPath, string remotePath)
    {
        var fileInfo = _repository.GetFileInfo(localPath);
        var fileStream = _repository.OpenFile(localPath);
        var splittedToBlocksStream = new StreamBlockReader(fileStream, fileInfo.Length);
        var blocks = new List<BlockInfo>();

        int index = 0;
        while (splittedToBlocksStream.HasNextBlock())
        {
            var block = await splittedToBlocksStream.GetNextBlockAsync();
            var blockName = Guid.NewGuid().ToString();
            var node = await _nodeContext.GetBestNodeAsync(block.Length);
            await _nodeFileClient.WriteBlockAsync(node, blockName, block);
            blocks.Add(new BlockInfo(index, node.Id, blockName, block.Length));
        }

        var remoteFileInfo = new RemoteFileInfo(remotePath, blocks);
        
        _fileInfoContext.RemoteFiles.Add(remoteFileInfo);
        await _fileInfoContext.SaveChangesAsync();
    }

    public async Task ReadFileAsync(string remotePath, string localPath)
    {
        var fileInfo = await _fileInfoContext.GetFileInfoAsync(remotePath);
        if (fileInfo is null)
        {
            throw new FileNotFoundException(remotePath);
        }

        _repository.CreateFile(localPath);
        foreach (var blockInfo in fileInfo.Blocks.OrderBy(b => b.Number))
        {
            var node = await _nodeContext.Nodes.FindAsync(blockInfo.NodeId);
            var blockContent = await _nodeFileClient.ReadBlockAsync(node, blockInfo.Name);
            await _repository.AppendFileContentAsync(localPath, blockContent);
        }
    }

    public async Task RemoveFileAsync(string remotePath)
    {
        var fileInfo = await _fileInfoContext.GetFileInfoAsync(remotePath);
        if (fileInfo is null)
        {
            throw new FileNotFoundException(remotePath);
        }

        foreach (var group in fileInfo.Blocks.GroupBy(b => b.NodeId))
        {
            var node = await _nodeContext.Nodes.FindAsync(group.Key);
            await _nodeFileClient.DeleteBlocksAsync(node, group.Select(g => g.Name).ToList());
        }

        _fileInfoContext.RemoteFiles.Remove(fileInfo);
        await _fileInfoContext.SaveChangesAsync();
    }
}