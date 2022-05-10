namespace DistFS.Node;

public class NodeConfigurationInfo
{
    public Guid Id { get; set; }
    public long Size { get; set; }
    public long FreeSpace { get; set; }
    public string Address { get; set; }
    public int Port { get; set; }
}