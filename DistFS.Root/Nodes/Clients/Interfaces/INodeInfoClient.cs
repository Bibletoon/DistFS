using DistFS.Models;

namespace DistFS.Nodes.Clients.Interfaces;

public interface INodeInfoClient
{
    NodeInfo Connect(string address, int port, string localName);
}