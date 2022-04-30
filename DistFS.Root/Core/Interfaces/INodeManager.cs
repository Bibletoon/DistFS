using DistFS.Models;

namespace DistFS.Core;

public interface INodeManager
{
    void RegisterNode(string name, string address, int port);
    NodeInfo GetBestNode(long requiredSize);
    NodeInfo? FindNode(Guid id);
    NodeInfo? FindNode(string name);
    void UpdateNodeFreeSpace(Guid nodeId, long newSpace);
}