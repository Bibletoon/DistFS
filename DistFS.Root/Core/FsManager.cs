using DistFS.Infrastructure;
using DistFS.Models;
using DistFS.Nodes.Clients.Interfaces;
using DistFS.Tools;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Core;

public class FsManager : IFsManager
{
    private readonly IFileRepository _repository;
    private readonly INodeManager _nodeManager;
    private readonly INodeFileClient _nodeFileClient;
    private readonly RootDbContext _context;
    
    public FsManager(IFileRepository repository, INodeManager nodeManager, INodeFileClient nodeFileClient, RootDbContext context)
    {
        _repository = repository;
        _nodeManager = nodeManager;
        _nodeFileClient = nodeFileClient;
        _context = context;
    }

    public void WriteFile(string localPath, string remotePath)
    {
        var content = _repository.ReadFile(localPath);
        var splitted = new ArraySplitter<byte>(content);
        var blocks = new List<BlockInfo>();
        int index = 0;
        while (splitted.HasNextBlock())
        {
            var block = splitted.GetNextBlock();
            var blockName = Guid.NewGuid().ToString();
            var node = _nodeManager.GetBestNode(block.Length);
            _nodeFileClient.WriteBlock(node, blockName, block);
            blocks.Add(new BlockInfo(index++, node.Id, blockName));
        }

        var fileInfo = new DistFileInfo(remotePath, blocks);
        _context.DistFiles.Add(fileInfo);
        _context.SaveChanges();
    }

    public void ReadFile(string remotePath, string localPath)
    {
        var fileInfo = _context.DistFiles.Include(d => d.Blocks).FirstOrDefault(c => c.RemotePath == remotePath);
        if (fileInfo is null)
        {
            throw new FileNotFoundException(remotePath);
        }

        var content = new List<byte>();
        foreach (var blockInfo in fileInfo.Blocks.OrderBy(b => b.Number))
        {
            var node = _nodeManager.FindNode(blockInfo.NodeId);
            var blockContent = _nodeFileClient.ReadBlock(node, blockInfo.BlockName);
            content.AddRange(blockContent);
        }
        _repository.WriteFile(localPath, content.ToArray());
    }

    public void RemoveFile(string remotePath)
    {
        var fileInfo = _context.DistFiles.Include(df => df.Blocks).FirstOrDefault(c => c.RemotePath == remotePath);
        if (fileInfo is null)
        {
            throw new FileNotFoundException(remotePath);
        }

        var content = new List<byte>();
        foreach (var blockInfo in fileInfo.Blocks.OrderBy(b => b.Number))
        {
            var node = _nodeManager.FindNode(blockInfo.NodeId);
            _nodeFileClient.DeleteBlock(node, blockInfo.BlockName);
        }
        
        _context.DistFiles.Remove(fileInfo);
    }
}