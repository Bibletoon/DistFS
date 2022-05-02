using DistFS.Infrastructure;
using DistFS.Models;
using DistFS.Nodes.Clients.Interfaces;
using DistFS.Tools.Exceptions;

namespace DistFS.Core;

public class NodeInfoManager : INodeInfoManager
{
    private readonly INodeInfoClient _nodeInfoClient;
    private readonly INodeContext _context;

    public NodeInfoManager(INodeInfoClient nodeInfoClient, RootDbContext context)
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
            _context.Nodes.Add(info);
        }

        _context.SaveChanges();
    }

    public void UpdateNodeFreeSpace(Guid nodeId, long newSpace)
    {
        var node = _context.Nodes.Find(nodeId);
        node.FreeSpace = newSpace;
        _context.Nodes.Update(node);
        _context.SaveChanges();
    }
}