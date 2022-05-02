using DistFS.Models;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Infrastructure.Database;

public interface INodeContext
{
    DbSet<NodeInfo> Nodes { get; set; }
    int SaveChanges();
    
    NodeInfo GetBestNode(long requiredSize)
    {
        return GetBestNode(requiredSize, Guid.Empty);
    }

    NodeInfo GetBestNode(long requiredSize, Guid except)
    {
        var nodeInfo = Nodes.Where(n => n.FreeSpace > requiredSize 
                                                 && n.Id != except)
            .OrderByDescending(n => (double)n.FreeSpace / n.Size)
            .First();
        return nodeInfo;
    }
}