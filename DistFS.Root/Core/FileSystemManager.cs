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

    public void WriteFile(string localPath, string remotePath)
    {
        var fileInfo = _repository.GetFileInfo(localPath);
        var fileStream = _repository.ReadFile(localPath);
        var splittedToBlocksStream = new StreamBlockReader(fileStream, fileInfo.Length);
        var blocks = new List<BlockInfo>();

        int index = 0;
        while (splittedToBlocksStream.HasNextBlock())
        {
            var block = splittedToBlocksStream.GetNextBlock();
            var blockName = Guid.NewGuid().ToString();
            var node = _nodeContext.GetBestNode(block.Length);
            _nodeFileClient.WriteBlock(node, blockName, block);
            blocks.Add(new BlockInfo(index, node.Id, blockName, block.Length));
        }

        var remoteFileInfo = new RemoteFileInfo(remotePath, blocks);
        
        _fileInfoContext.RemoteFiles.Add(remoteFileInfo);
        _fileInfoContext.SaveChanges();
    }

    public void ReadFile(string remotePath, string localPath)
    {
        var fileInfo = _fileInfoContext.GetFileInfo(remotePath);
        if (fileInfo is null)
        {
            throw new FileNotFoundException(remotePath);
        }

        var content = new List<byte>();
        foreach (var blockInfo in fileInfo.Blocks.OrderBy(b => b.Number))
        {
            var node = _nodeContext.Nodes.Find(blockInfo.NodeId);
            var blockContent = _nodeFileClient.ReadBlock(node, blockInfo.Name);
            content.AddRange(blockContent);
        }
        _repository.WriteFile(localPath, content.ToArray());
    }

    public void RemoveFile(string remotePath)
    {
        var fileInfo = _fileInfoContext.GetFileInfo(remotePath);
        if (fileInfo is null)
        {
            throw new FileNotFoundException(remotePath);
        }

        foreach (var blockInfo in fileInfo.Blocks.OrderBy(b => b.Number))
        {
            var node = _nodeContext.Nodes.Find(blockInfo.NodeId);
            _nodeFileClient.DeleteBlock(node, blockInfo.Name);
        }

        _fileInfoContext.RemoteFiles.Remove(fileInfo);
        _fileInfoContext.SaveChanges();
    }
}