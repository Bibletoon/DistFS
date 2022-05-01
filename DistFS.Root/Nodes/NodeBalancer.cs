using DistFS.Core;
using DistFS.Infrastructure;
using DistFS.Nodes.Clients.Interfaces;
using DistFS.Tools.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Nodes;

public class NodeBalancer : INodeBalancer
{
    private readonly INodeManager _nodeManager;
    private readonly RootDbContext _context;
    private readonly INodeFileClient _fileClient;
    
    public NodeBalancer(INodeManager nodeManager, RootDbContext context, INodeFileClient fileClient)
    {
        _nodeManager = nodeManager;
        _context = context;
        _fileClient = fileClient;
    }

    public void RebalanceNodes()
    {
        // var nodesInfo = _context.Nodes
        //     .Select(n => new { nodeId = n.Id, workload  = (double)n.FreeSpace / n.Size })
        //     .ToList();
        // var avg = nodesInfo.Average(n => n.workload);
        throw new NotImplementedException();
    }

    public void CleanNode(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        
        var node = _nodeManager.GetNode(name);

        var blocks = _context.Blocks.Where(b => b.NodeId == node.Id).ToList();
        foreach (var block in blocks)
        {
            var blockData = _fileClient.ReadBlock(node, block.Name);
            _fileClient.DeleteBlock(node, block.Name);
            var newNode = _nodeManager.GetBestNode(blockData.Length, block.NodeId);
            _fileClient.WriteBlock(newNode, block.Name, blockData);
            block.NodeId = newNode.Id;
        }
        _context.Blocks.UpdateRange(blocks);
        _context.SaveChanges();
    }
}