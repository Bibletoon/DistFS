using DistFS.Models;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Infrastructure.Database;

public interface INodeContext
{
    DbSet<NodeInfo> Nodes { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    async Task<NodeInfo> GetBestNodeAsync(long requiredSize)
    {
        return await GetBestNodeAsync(requiredSize, Guid.Empty);
    }

    async Task<NodeInfo> GetBestNodeAsync(long requiredSize, Guid except)
    {
        var nodeInfo = await Nodes.Where(n => n.FreeSpace > requiredSize 
                                                 && n.Id != except)
            .OrderByDescending(n => (double)n.FreeSpace / n.Size)
            .FirstAsync();
        return nodeInfo;
    }
}