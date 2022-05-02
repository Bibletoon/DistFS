using DistFS.Core.Interfaces;
using DistFS.Infrastructure;
using DistFS.Infrastructure.Database;
using DistFS.Models;
using DistFS.Nodes.Clients.Interfaces;
using DistFS.Tools.Exceptions;

namespace DistFS.Core;

public class NodeManager : INodeManager
{
    private readonly INodeInfoClient _nodeInfoClient;
    private readonly INodeFileClient _fileClient;
    private readonly INodeContext _nodeContext;
    private readonly IBlockContext _blockContext;

    public NodeManager(INodeInfoClient nodeInfoClient, RootDbContext nodeContext, IBlockContext blockContext, INodeFileClient fileClient)
    {
        _nodeInfoClient = nodeInfoClient;
        _nodeContext = nodeContext;
        _blockContext = blockContext;
        _fileClient = fileClient;
    }

    public void RegisterNode(string name, string address, int port)
    {
        var info = _nodeInfoClient.Connect(address, port, name);
        if (_nodeContext.Nodes.Any(n => n.Id == info.Id))
        {
            _nodeContext.Nodes.Update(info);
        }
        else
        {
            _nodeContext.Nodes.Add(info);
        }

        _nodeContext.SaveChanges();
    }

    public void UpdateNodeFreeSpace(Guid nodeId, long newSpace)
    {
        var node = _nodeContext.Nodes.Find(nodeId);
        node.FreeSpace = newSpace;
        _nodeContext.Nodes.Update(node);
        _nodeContext.SaveChanges();
    }

    public void CleanNode(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        
        var node = _nodeContext.Nodes.First(n => n.Name == name);

        var blocks = _blockContext.Blocks.Where(b => b.NodeId == node.Id);
        foreach (var block in blocks)
        {
            var blockData = _fileClient.ReadBlock(node, block.Name);
            _fileClient.DeleteBlock(node, block.Name);
            var newNode = _nodeContext.GetBestNode(blockData.Length, block.NodeId);
            _fileClient.WriteBlock(newNode, block.Name, blockData);
            _blockContext.UpdateBlockNode(block.Name, newNode.Id);
        }
    }
}