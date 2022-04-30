namespace DistFS.Models;

public class BlockInfo
{
    public int Number { get; private init; }
    public Guid NodeId { get; private init; }
    public string BlockName { get; private init; }

    private BlockInfo()
    { }

    public BlockInfo(int number, Guid nodeId, string blockName)
    {
        Number = number;
        NodeId = nodeId;
        BlockName = blockName;
    }
}