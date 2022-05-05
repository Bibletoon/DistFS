using DistFS.Models;
using DistFS.Tools.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Infrastructure.Database;

public interface IBlockContext
{
    DbSet<BlockInfo> Blocks { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    IEnumerable<EnumeratedBlock> EnumerateBlocks()
    {
        return Blocks.AsEnumerable().GroupBy(b => b.NodeId)
            .SelectMany(g => g.Select((b, n) => new EnumeratedBlock(b, n)))
            .OrderBy(i => i.Number)
            .ThenBy(i => i.Block.NodeId);
    }
    
    async Task UpdateBlockNodeAsync(string blockName, Guid nodeId)
    {
        var block = await Blocks.FirstOrDefaultAsync(b => b.Name == blockName);

        if (block is null)
            throw new BlockNotFoundException($"Block with name ${blockName} not found");
        
        block.NodeId = nodeId;
        Blocks.Update(block);
        await SaveChangesAsync();
    }
}