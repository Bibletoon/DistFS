using DistFS.Models;

namespace DistFS.Nodes.Clients.Interfaces;

public interface INodeFileClient
{
    Task WriteBlockAsync(NodeInfo node, string blockName, byte[] block);
    Task<byte[]> ReadBlockAsync(NodeInfo node, string blockName);
    Task DeleteBlocksAsync(NodeInfo node, List<string> blocks);
    Task<byte[]> ExtractBlockAsync(NodeInfo node, string blockName);
}