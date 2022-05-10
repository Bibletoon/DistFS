namespace DistFS.Node.Services.Interfaces;

public interface INodeConfigurationProvider
{
    NodeConfigurationInfo GetConfiguration();
    void IncreaseFreeSpace(long space);
    void DecreaseFreeSpace(long space);
    long GetFreeSpace();
}