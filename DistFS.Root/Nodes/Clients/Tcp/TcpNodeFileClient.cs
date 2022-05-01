using System.Net.Sockets;
using DistFS.Core;
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
    
    public void WriteBlock(NodeInfo node, string blockName, byte[] block)
    {
        var command = new WriteBlockCommand(blockName, block);
        var newFreeSpaceBytes = SendCommandAndReceiveBytes(node, command);
        var newFreeSpace = BitConverter.ToInt64(newFreeSpaceBytes);
        _nodeManager.UpdateNodeFreeSpace(node.Id, newFreeSpace);
    }

    public byte[] ReadBlock(NodeInfo node, string blockName)
    {
        var command = new ReadBlockCommand(blockName);
        var block = SendCommandAndReceiveBytes(node, command);
        return block;
    }

    public void DeleteBlock(NodeInfo node, string blockName)
    {
        var command = new DeleteBlockCommand(blockName);
        var newFreeSpaceBytes = SendCommandAndReceiveBytes(node, command);
        var newFreeSpace = BitConverter.ToInt64(newFreeSpaceBytes);
        _nodeManager.UpdateNodeFreeSpace(node.Id, newFreeSpace);
    }

    private byte[] SendCommandAndReceiveBytes(NodeInfo node, Command command)
    {
        var client = new TcpClient();
        client.Connect(node.Address, node.Port);
        var stream = client.GetStream();
        stream.SendCommand(command);
        var bytes = stream.AcceptBytes();
        stream.Close();
        client.Close();
        return bytes;
    }
}