using DistFS.Core.Interfaces;
using DistFS.Infrastructure.Database;
using DistFS.Nodes.Clients.Interfaces;

namespace DistFS.Core;

public class NodeManager : INodeManager
{
    private readonly INodeInfoClient _nodeInfoClient;
    private readonly INodeContext _nodeContext;
    private readonly IBlockContext _blockContext;

    public NodeManager(INodeInfoClient nodeInfoClient, RootDbContext nodeContext, IBlockContext blockContext)
    {
        _nodeInfoClient = nodeInfoClient;
        _nodeContext = nodeContext;
        _blockContext = blockContext;
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
        var node = _nodeContext.Nodes.First(n => n.Id == nodeId);
        node.FreeSpace = newSpace;
        _nodeContext.Nodes.Update(node);
        _nodeContext.SaveChanges();
    }
}