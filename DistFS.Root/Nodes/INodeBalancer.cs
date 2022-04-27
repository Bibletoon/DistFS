namespace DistFS.Nodes;

public interface INodeBalancer
{
    void RebalanceNodes();
    void CleanNode(string name);
}