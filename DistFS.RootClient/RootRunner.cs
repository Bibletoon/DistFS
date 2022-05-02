using DistFS.Core;
using DistFS.Core.Interfaces;
using DistFS.Infrastructure;
using DistFS.Infrastructure.Database;
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
        var manager = provider.GetRequiredService<INodeInfoManager>();
        manager.RegisterNode("Some node", "127.0.0.1", 8081);
        manager.RegisterNode("Other node", "127.0.0.1", 8082);
        manager.RegisterNode("Another node", "127.0.0.1", 8083);
        var fsManager = provider.GetRequiredService<IFileSystemManager>();
        fsManager.WriteFile("pic.jpg", "kek/lol/pic.jpg");
        // fsManager.ReadFile("kek/lol/pic.jpg", "lol.jpg");
        fsManager.RemoveFile("kek/lol/pic.jpg");
    }

    public void ConfigureServices(ServiceCollection collection)
    {
        collection.AddDbContext<RootDbContext>(o => o.UseSqlite("Filename=root.db"));
        collection.AddScoped<IFileSystemManager, FileSystemManager>();
        collection.AddScoped<INodeInfoManager, NodeInfoManager>();
        collection.AddScoped<IFileRepository, LocalFileRepository>();
        collection.AddScoped<INodeFileClient, TcpNodeFileClient>();
        collection.AddScoped<INodeInfoClient, TcpNodeInfoClient>();
    }
}