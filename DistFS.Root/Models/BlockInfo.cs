namespace DistFS.Models;

public class BlockInfo
{
    public int Number { get; private init; }
    public Guid NodeId { get; set; }
    public string Name { get; private init; }

    private BlockInfo()
    { }

    public BlockInfo(int number, Guid nodeId, string name)
    {
        Number = number;
        NodeId = nodeId;
        Name = name;
    }
}