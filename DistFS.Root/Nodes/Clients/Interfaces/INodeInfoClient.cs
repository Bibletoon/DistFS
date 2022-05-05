using DistFS.Models;

namespace DistFS.Nodes.Clients.Interfaces;

public interface INodeInfoClient
{
    Task<NodeInfo> ConnectAsync(string address, int port, string localName);
}