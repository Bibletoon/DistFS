namespace DistFS.Nodes;

public interface INodeWorkloadManager
{
    void RebalanceNodes();
    void CleanNode(string name);
}