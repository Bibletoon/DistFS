using DistFS.Models;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Infrastructure;

public class RootDbContext : DbContext
{
    public DbSet<NodeInfo> Nodes { get; set; }
    public DbSet<DistFileInfo> DistFiles { get; set; }

    public RootDbContext(DbContextOptions<RootDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DistFileInfo>().HasMany(f => f.Blocks);
        modelBuilder.Entity<BlockInfo>().HasKey(b => b.BlockName);
        modelBuilder.Entity<DistFileInfo>().HasKey(df => df.RemotePath);
        modelBuilder.Entity<NodeInfo>().HasKey(n => n.Id);
    }
}