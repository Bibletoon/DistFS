using DistFS.Infrastructure;
using DistFS.Models;
using DistFS.Nodes.Clients.Interfaces;
using DistFS.Tools;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Core;

public class FileSystemManager : IFileSystemManager
{
    private readonly IFileRepository _repository;
    private readonly INodeManager _nodeManager;
    private readonly INodeFileClient _nodeFileClient;
    private readonly IFileInfoManager _fileInfoManager;
    
    public FileSystemManager(IFileRepository repository, INodeManager nodeManager, INodeFileClient nodeFileClient, IFileInfoManager fileInfoManager)
    {
        _repository = repository;
        _nodeManager = nodeManager;
        _nodeFileClient = nodeFileClient;
        _fileInfoManager = fileInfoManager;
    }

    public void WriteFile(string localPath, string remotePath)
    {
        var fileContent = _repository.ReadFile(localPath);
        var splittedToBlocksArray = new ArraySplitter<byte>(fileContent);
        var blocks = new List<BlockInfo>();

        foreach (var (block, index) in splittedToBlocksArray.Select((value, i) => (value, i)))
        {
            var blockName = Guid.NewGuid().ToString();
            var node = _nodeManager.GetBestNode(block.Length);
            _nodeFileClient.WriteBlock(node, blockName, block);
            blocks.Add(new BlockInfo(index, node.Id, blockName));
        }

        var fileInfo = new RemoteFileInfo(remotePath, blocks);
        _fileInfoManager.AddFile(fileInfo);
    }

    public void ReadFile(string remotePath, string localPath)
    {
        var fileInfo = _fileInfoManager.GetFileInfo(remotePath);
        if (fileInfo is null)
        {
            throw new FileNotFoundException(remotePath);
        }

        var content = new List<byte>();
        foreach (var blockInfo in fileInfo.Blocks.OrderBy(b => b.Number))
        {
            var node = _nodeManager.GetNode(blockInfo.NodeId);
            var blockContent = _nodeFileClient.ReadBlock(node, blockInfo.Name);
            content.AddRange(blockContent);
        }
        _repository.WriteFile(localPath, content.ToArray());
    }

    public void RemoveFile(string remotePath)
    {
        var fileInfo = _fileInfoManager.GetFileInfo(remotePath);
        if (fileInfo is null)
        {
            throw new FileNotFoundException(remotePath);
        }

        foreach (var blockInfo in fileInfo.Blocks.OrderBy(b => b.Number))
        {
            var node = _nodeManager.GetNode(blockInfo.NodeId);
            _nodeFileClient.DeleteBlock(node, blockInfo.Name);
        }

        _fileInfoManager.RemoveFile(fileInfo);
    }
}