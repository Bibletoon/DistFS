namespace DistFs.Tcp.Common.NodeAbstractions;

public interface INodeConfigurationProvider
{
    NodeConfigurationInfo GetConfiguration();
    void IncreaseFreeSpace(long space);
    void DecreaseFreeSpace(long space);
    long GetFreeSpace();
}