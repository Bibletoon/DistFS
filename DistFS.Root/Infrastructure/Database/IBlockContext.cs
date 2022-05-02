using DistFS.Models;
using DistFS.Tools.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Infrastructure;

public interface IBlockContext
{
    DbSet<BlockInfo> Blocks { get; set; }
    int SaveChanges();
    
    IEnumerable<EnumeratedBlock> EnumerateBlocks()
    {
        return Blocks.GroupBy(b => b.NodeId)
            .SelectMany(g => g.Select((b, n) => new EnumeratedBlock(b, n)))
            .OrderBy(i => i.Number)
            .ThenBy(i => i.Block.NodeId);
    }
    
    void UpdateBlockNode(string blockName, Guid nodeId)
    {
        var block = Blocks.FirstOrDefault(b => b.Name == blockName);

        if (block is null)
            throw new BlockNotFoundException($"Block with name ${blockName} not found");
        
        block.NodeId = nodeId;
        Blocks.Update(block);
        SaveChanges();
    }
}