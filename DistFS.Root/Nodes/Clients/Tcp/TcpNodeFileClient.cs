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
        var client = new TcpClient();
        client.Connect(node.Address, node.Port);
        var stream = client.GetStream();
        stream.SendCommand(new WriteBlockCommand(blockName, block));
        var newFreeSpaceBytes = stream.AcceptBytes();
        var newFreeSpace = BitConverter.ToInt64(newFreeSpaceBytes);
        stream.Close();
        client.Close();
        _nodeManager.UpdateNodeFreeSpace(node.Id, newFreeSpace);
    }

    public byte[] ReadBlock(NodeInfo node, string blockName)
    {
        var client = new TcpClient();
        client.Connect(node.Address, node.Port);
        var stream = client.GetStream();
        stream.SendCommand(new ReadBlockCommand(blockName));
        var block = stream.AcceptBytes();
        stream.Close();
        client.Close();
        return block;
    }

    public void DeleteBlock(NodeInfo node, string blockName)
    {
        var client = new TcpClient();
        client.Connect(node.Address, node.Port);
        var stream = client.GetStream();
        stream.SendCommand(new DeleteBlockCommand(blockName));
        var newFreeSpaceBytes = stream.AcceptBytes();
        var newFreeSpace = BitConverter.ToInt64(newFreeSpaceBytes);
        stream.Close();
        client.Close();
        _nodeManager.UpdateNodeFreeSpace(node.Id, newFreeSpace);
    }
}