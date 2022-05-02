using DistFS.Core;
using DistFS.Core.Interfaces;
using DistFS.Infrastructure;
using DistFS.Infrastructure.Database;
using DistFS.Nodes;
using DistFS.Nodes.Clients.Interfaces;
using DistFS.Nodes.Clients.Tcp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DistFS.RootClient;

public class RootRunner
{
    public void Run()
    {
        var collection = new ServiceCollection();
        ConfigureServices(collection);
        var provider = collection.BuildServiceProvider();
        var manager = provider.GetRequiredService<INodeManager>();
        manager.RegisterNode("Some node", "127.0.0.1", 8081);
        var fsManager = provider.GetRequiredService<IFileSystemManager>();
        fsManager.WriteFile("pic.jpg", "kek/lol/pic.jpg");
        // fsManager.ReadFile("kek/lol/pic.jpg", "lol.jpg");
        manager.RegisterNode("Other node", "127.0.0.1", 8082);
        var balancer = provider.GetRequiredService<INodeWorkloadManager>();
        balancer.RebalanceNodes();
        fsManager.ReadFile("kek/lol/pic.jpg", "result.jpg");
    }

    public void ConfigureServices(ServiceCollection collection)
    {
        collection.AddDbContext<RootDbContext>(o => o.UseSqlite("Filename=root.db"));
        collection.AddScoped<INodeContext>(c => c.GetRequiredService<RootDbContext>());
        collection.AddScoped<IFileInfoContext>(c => c.GetRequiredService<RootDbContext>());
        collection.AddScoped<IBlockContext>(c => c.GetRequiredService<RootDbContext>());
        collection.AddScoped<IFileSystemManager, FileSystemManager>();
        collection.AddScoped<INodeManager, NodeManager>();
        collection.AddScoped<IFileRepository, LocalFileRepository>();
        collection.AddScoped<INodeFileClient, TcpNodeFileClient>();
        collection.AddScoped<INodeInfoClient, TcpNodeInfoClient>();
        collection.AddScoped<INodeWorkloadManager, NodeWorkloadManager>();
    }
}