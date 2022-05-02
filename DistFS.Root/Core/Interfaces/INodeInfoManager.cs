namespace DistFS.Core.Interfaces;

public interface INodeInfoManager
{
    void RegisterNode(string name, string address, int port);
    void UpdateNodeFreeSpace(Guid nodeId, long newSpace);
}