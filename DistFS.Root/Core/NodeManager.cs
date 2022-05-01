using DistFS.Infrastructure;
using DistFS.Models;
using DistFS.Nodes.Clients.Interfaces;
using DistFS.Tools.Exceptions;

namespace DistFS.Core;

public class NodeManager : INodeManager
{
    private readonly INodeInfoClient _nodeInfoClient;
    private readonly RootDbContext _context;

    public NodeManager(INodeInfoClient nodeInfoClient, RootDbContext context)
    {
        _nodeInfoClient = nodeInfoClient;
        _context = context;
    }

    public void RegisterNode(string name, string address, int port)
    {
        var info = _nodeInfoClient.Connect(address, port, name);
        if (_context.Nodes.Any(n => n.Id == info.Id))
        {
            _context.Nodes.Update(info);
        }
        else
        {
            _context.Add(info);
        }

        _context.SaveChanges();
    }

    public NodeInfo GetBestNode(long requiredSize)
    {
        return GetBestNode(requiredSize, Guid.Empty);
    }

    public NodeInfo GetBestNode(long requiredSize, Guid except)
    {
        var nodeInfo = _context.Nodes.Where(n => n.FreeSpace > requiredSize 
                                                        && n.Id != except)
            .OrderByDescending(n => (double)n.FreeSpace / n.Size)
            .First();
        return nodeInfo;
    }

    public NodeInfo GetNode(Guid id)
    {
        return _context.Nodes.Find(id) 
               ?? throw new NodeNotFoundException($"Node with ID {id} not found");
    }

    public NodeInfo GetNode(string name)
    {
       return _context.Nodes.FirstOrDefault(n => n.Name == name) 
              ?? throw new NodeNotFoundException($"Node with name {name} not found");
    }

    public void UpdateNodeFreeSpace(Guid nodeId, long newSpace)
    {
        var node = GetNode(nodeId);
        node.FreeSpace = newSpace;
        _context.Nodes.Update(node);
        _context.SaveChanges();
    }
}