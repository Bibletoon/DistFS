namespace DistFS.Models;

public class NodeInfo
{
    public Guid Id { get; }
    public string Name { get; }
    public string Address { get; }
    public int Port { get; }
    private int Size { get; }
    public int FreeSpace  { get; private set; }

    public NodeInfo(Guid id, string name, string address, int port, int size)
        : this(id, name, address, port, size, size)
    { }

    public NodeInfo(Guid id, string name, string address, int port, int size, int freeSpace)
    {
        Id = id;
        Name = name;
        Address = address;
        Port = port;
        Size = size;
        FreeSpace = freeSpace;
    }

    public void UpdateFreeSpace(int size)
    {
        FreeSpace = size;
    } 
}