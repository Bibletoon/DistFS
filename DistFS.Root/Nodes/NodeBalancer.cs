using DistFS.Core;
using DistFS.Infrastructure;
using DistFS.Infrastructure.Database;
using DistFS.Nodes.Clients.Interfaces;
using DistFS.Tools.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Nodes;

public class NodeBalancer : INodeBalancer
{
    private readonly INodeContext _nodeContext;
    private readonly IBlockContext _blockContext;
    private readonly INodeFileClient _fileClient;

    public NodeBalancer(INodeFileClient fileClient, INodeContext nodeContext, IBlockContext blockContext)
    {
        _fileClient = fileClient;
        _nodeContext = nodeContext;
        _blockContext = blockContext;
    }

    public void RebalanceNodes()
    {
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