using System.Net.Sockets;
using DistFS.Models;
using DistFS.Nodes.Clients.Interfaces;
using DistFs.Tcp.Common;
using DistFs.Tcp.Common.Dto;

namespace DistFS.Nodes.Clients.Tcp;

public class TcpNodeInfoClient : INodeInfoClient
{
    public NodeInfo Connect(string address, int port, string localName)
    {
        var client = new TcpClient();
        client.Connect(address, port);
        var stream = client.GetStream();
        stream.SendCommand(new GetNodeConfigurationCommand());
        var configuration = stream.Accept<NodeConfigurationDto>();
        var nodeInfo = new NodeInfo(
            configuration.Id,
            localName,
            address,
            port,
            configuration.Size,
            configuration.FreeSize);
        stream.Close();
        client.Close();
        return nodeInfo;
    }
}