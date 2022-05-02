namespace DistFS.Core.Interfaces;

public interface INodeManager
{
    void RegisterNode(string name, string address, int port);
    void UpdateNodeFreeSpace(Guid nodeId, long newSpace);
    void CleanNode(string name);
}