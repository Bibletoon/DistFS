using DistFS.Infrastructure;
using DistFS.Models;
using DistFS.Nodes.Clients.Interfaces;

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
        var nodeInfo = _context.Nodes.Where(n => n.FreeSpace > requiredSize)
                                     .OrderByDescending(n => (double)n.FreeSpace / n.Size)
                                     .First();
        return nodeInfo;
    }

    public NodeInfo? FindNode(Guid id)
    {
        return _context.Nodes.Find(id);
    }

    public NodeInfo? FindNode(string name)
    {
       return _context.Nodes.FirstOrDefault(n => n.Name == name);
    }

    public void UpdateNodeFreeSpace(Guid nodeId, long newSpace)
    {
        var node = _context.Nodes.Find(nodeId);
        node.FreeSpace = newSpace;
        _context.Nodes.Update(node);
        _context.SaveChanges();
    }
}