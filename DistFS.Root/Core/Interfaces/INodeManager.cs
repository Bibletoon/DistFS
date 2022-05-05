namespace DistFS.Core.Interfaces;

public interface INodeManager
{
    Task RegisterNodeAsync(string name, string address, int port);
    Task UpdateNodeFreeSpaceAsync(Guid nodeId, long newSpace);
}