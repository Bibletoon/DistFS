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
}