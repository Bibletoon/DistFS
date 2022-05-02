using DistFS.Models;

namespace DistFS.Core;

public interface INodeInfoManager
{
    void RegisterNode(string name, string address, int port);
    void UpdateNodeFreeSpace(Guid nodeId, long newSpace);
}