using DistFS.Core.Interfaces;
using DistFS.Infrastructure.Database;
using DistFS.Nodes.Clients.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Core;

public class NodeManager : INodeManager
{
    private readonly INodeInfoClient _nodeInfoClient;
    private readonly INodeContext _nodeContext;

    public NodeManager(INodeInfoClient nodeInfoClient, RootDbContext nodeContext)
    {
        _nodeInfoClient = nodeInfoClient;
        _nodeContext = nodeContext;
    }

    public async Task RegisterNodeAsync(string name, string address, int port)
    {
        var info = await _nodeInfoClient.ConnectAsync(address, port, name);
        if (await _nodeContext.Nodes.AnyAsync(n => n.Id == info.Id))
        {
            _nodeContext.Nodes.Update(info);
        }
        else
        {
            await _nodeContext.Nodes.AddAsync(info);
        }

        await _nodeContext.SaveChangesAsync();
    }

    public async Task UpdateNodeFreeSpaceAsync(Guid nodeId, long newSpace)
    {
        var node = await _nodeContext.Nodes.FindAsync(nodeId);
        node.FreeSpace = newSpace;
        _nodeContext.Nodes.Update(node);
        await _nodeContext.SaveChangesAsync();
    }
}