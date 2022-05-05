namespace DistFS.Nodes;

public interface INodeWorkloadManager
{
    Task RebalanceNodesAsync();
    Task CleanNodeAsync(string name);
}