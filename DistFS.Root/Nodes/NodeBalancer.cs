using DistFS.Core;

namespace DistFS.Nodes;

public class NodeBalancer : INodeBalancer
{
    private readonly INodeManager _nodeManager;

    public NodeBalancer(INodeManager nodeManager)
    {
        _nodeManager = nodeManager;
    }

    public void RebalanceNodes()
    {
        throw new NotImplementedException();
    }

    public void CleanNode(string name)
    {
        _nodeManager.FindNode(name);
    }
}