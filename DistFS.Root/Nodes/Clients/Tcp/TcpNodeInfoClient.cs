using System.Net.Sockets;
using DistFS.Models;
using DistFS.Nodes.Clients.Interfaces;
using DistFs.Tcp.Common;
using DistFs.Tcp.Common.Dto;

namespace DistFS.Nodes.Clients.Tcp;

public class TcpNodeInfoClient : INodeInfoClient
{
    public async Task<NodeInfo> ConnectAsync(string address, int port, string localName)
    {
        var client = new TcpClient();
        await client.ConnectAsync(address, port);
        var stream = client.GetStream();
        await stream.SendCommandAsync(new GetNodeConfigurationCommand());
        var configuration = await stream.AcceptAsync<NodeConfigurationDto>();
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