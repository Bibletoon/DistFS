using System.Net.Sockets;
using DistFS.Core.Interfaces;
using DistFS.Models;
using DistFS.Nodes.Clients.Interfaces;
using DistFs.Tcp.Common;

namespace DistFS.Nodes.Clients.Tcp;

public class TcpNodeFileClient : INodeFileClient
{
    private readonly INodeManager _nodeManager;

    public TcpNodeFileClient(INodeManager nodeManager)
    {
        _nodeManager = nodeManager;
    }
    
    public async Task WriteBlockAsync(NodeInfo node, string blockName, byte[] block)
    {
        var command = new WriteBlockCommand(blockName, block.ToArray());
        var newFreeSpaceBytes = await SendCommandAndReceiveBytesAsync(node, command);
        var newFreeSpace = BitConverter.ToInt64(newFreeSpaceBytes);
        await _nodeManager.UpdateNodeFreeSpaceAsync(node.Id, newFreeSpace);
    }

    public async Task<byte[]> ReadBlockAsync(NodeInfo node, string blockName)
    {
        var command = new ReadBlockCommand(blockName);
        var block = await SendCommandAndReceiveBytesAsync(node, command);
        return block;
    }

    public async Task DeleteBlocksAsync(NodeInfo node, List<string> blocks)
    {
        var command = new DeleteBlocksCommand(blocks);
        var newFreeSpaceBytes = await SendCommandAndReceiveBytesAsync(node, command);
        var newFreeSpace = BitConverter.ToInt64(newFreeSpaceBytes);
        await _nodeManager.UpdateNodeFreeSpaceAsync(node.Id, newFreeSpace);
    }

    public async Task<byte[]> ExtractBlockAsync(NodeInfo node, string blockName)
    {
        var command = new ExtractBlockCommand(blockName);
        var client = new TcpClient();
        await client.ConnectAsync(node.Address, node.Port);
        var stream = client.GetStream();
        await stream.SendCommandAsync(command);
        var bytes = await stream.AcceptBytesAsync();
        var newFreeSpaceBytes = await stream.AcceptBytesAsync();
        var newFreeSpace = BitConverter.ToInt64(newFreeSpaceBytes);
        await _nodeManager.UpdateNodeFreeSpaceAsync(node.Id, newFreeSpace);
        stream.Close();
        client.Close();
        return bytes;
    }

    private async Task<byte[]> SendCommandAndReceiveBytesAsync(NodeInfo node, Command command)
    {
        var client = new TcpClient();
        await client.ConnectAsync(node.Address, node.Port);
        var stream = client.GetStream();
        await stream.SendCommandAsync(command);
        var bytes = await stream.AcceptBytesAsync();
        stream.Close();
        client.Close();
        return bytes;
    }
}