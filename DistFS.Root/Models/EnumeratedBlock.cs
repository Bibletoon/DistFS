namespace DistFS.Models;

public class EnumeratedBlock
{
    public BlockInfo Block { get; }
    public int Number { get; }

    public EnumeratedBlock(BlockInfo block, int number)
    {
        Block = block;
        Number = number;
    }
}