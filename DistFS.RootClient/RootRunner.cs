using DistFS.Core;
using DistFS.Core.Interfaces;
using DistFS.Infrastructure;
using DistFS.Infrastructure.Database;
using DistFS.Nodes;
using DistFS.Nodes.Clients.Interfaces;
using DistFS.Nodes.Clients.Tcp;
using DistFS.RootClient.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DistFS.RootClient;

public class RootRunner
{
    public void Run()
    {
        var collection = new ServiceCollection();
        ConfigureServices(collection);
        var typeProvider = ConfigureCommands(collection);
        collection.AddSingleton(typeProvider);
        var provider = collection.BuildServiceProvider();
        // while (true)
        // {
        //     var commandRequest = Console.ReadLine();
        //     var commandName = string.Join("", commandRequest.Split(' ').First().Skip(1));
        //     var commandArguments = commandRequest.Split(' ').Skip(1).ToArray();
        //
        //     if (commandName == "exit")
        //         break;
        //     
        //     var commandType = typeProvider.GetCommandType(commandName);
        //     var command = (Command)provider.GetRequiredService(commandType);
        //     command.Execute(commandArguments);
        // }

        var command = provider.GetRequiredService<ExecuteCommand>();
        command.Execute(new [] {"commands.txt"});
    }

    private void ConfigureServices(IServiceCollection collection)
    {
        collection.AddDbContext<RootDbContext>(o => o.UseInMemoryDatabase("root.db"));
        collection.AddScoped<INodeContext>(c => c.GetRequiredService<RootDbContext>());
        collection.AddScoped<IFileInfoContext>(c => c.GetRequiredService<RootDbContext>());
        collection.AddScoped<IBlockContext>(c => c.GetRequiredService<RootDbContext>());
        collection.AddScoped<IFileSystemManager, FileSystemManager>();
        collection.AddScoped<INodeManager, NodeManager>();
        collection.AddScoped<IFileRepository, LocalFileRepository>();
        collection.AddScoped<INodeFileClient, TcpNodeFileClient>();
        collection.AddScoped<INodeInfoClient, TcpNodeInfoClient>();
        collection.AddScoped<INodeWorkloadManager, NodeWorkloadManager>();
        collection.AddSingleton(sp => sp);
    }

    private CommandTypeProvider ConfigureCommands(IServiceCollection collection)
    {
        collection
            .AddScoped<AddFileCommand>()
            .AddScoped<AddNodeCommand>()
            .AddScoped<BalanceNodesCommand>()
            .AddScoped<CleanNodeCommand>()
            .AddScoped<ReadFileCommand>()
            .AddScoped<RemoveFileCommand>()
            .AddScoped<ExecuteCommand>();

        var typeProvider = new CommandTypeProvider();
        typeProvider
            .RegisterCommand<AddFileCommand>()
            .RegisterCommand<AddNodeCommand>()
            .RegisterCommand<BalanceNodesCommand>()
            .RegisterCommand<CleanNodeCommand>()
            .RegisterCommand<ReadFileCommand>()
            .RegisterCommand<RemoveFileCommand>()
            .RegisterCommand<ExecuteCommand>();

        return typeProvider;
    }
}