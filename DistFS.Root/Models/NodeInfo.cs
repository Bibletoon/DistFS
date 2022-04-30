namespace DistFS.Models;

public class NodeInfo
{
    public Guid Id { get; private init; }
    public string Name { get; private init; }
    public string Address { get; private init; }
    public int Port { get; private init; }
    public long Size { get; private init; }
    public long FreeSpace { get; set; }

    private NodeInfo() { }
    
    public NodeInfo(Guid id, string name, string address, int port, long size, long freeSpace)
    {
        Id = id;
        Name = name;
        Address = address;
        Port = port;
        Size = size;
        FreeSpace = freeSpace;
    }
}