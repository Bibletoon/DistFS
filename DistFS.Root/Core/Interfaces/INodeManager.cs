using DistFS.Models;

namespace DistFS.Core;

public interface INodeManager
{
    void RegisterNode(string name, string address, int port);
    NodeInfo GetBestNode(long requiredSize);
    NodeInfo GetBestNode(long requiredSize, Guid except);
    NodeInfo GetNode(Guid id);
    NodeInfo GetNode(string name);
    void UpdateNodeFreeSpace(Guid nodeId, long newSpace);
}