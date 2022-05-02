using DistFS.Models;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Infrastructure;

public sealed class RootDbContext : DbContext, INodeContext, IBlockContext, IFileInfoContext
{
    public DbSet<NodeInfo> Nodes { get; set; }
    public DbSet<RemoteFileInfo> RemoteFiles { get; set; }
    public DbSet<BlockInfo> Blocks { get; set; }

    public RootDbContext(DbContextOptions<RootDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RemoteFileInfo>().HasMany(f => f.Blocks);
        modelBuilder.Entity<BlockInfo>().HasKey(b => b.Name);
        modelBuilder.Entity<RemoteFileInfo>().HasKey(df => df.RemotePath);
        modelBuilder.Entity<NodeInfo>().HasKey(n => n.Id);
    }
}