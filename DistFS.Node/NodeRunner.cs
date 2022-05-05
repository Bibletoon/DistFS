using System.ComponentModel.Design;
using System.Net;
using System.Net.Sockets;
using DistFs.Tcp.Common;
using DistFs.Tcp.Common.NodeAbstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DistFS.Node;

public class TcpNodeRunner
{
    public void Run()
    {
        var collection = new ServiceCollection();
        ConfigureServices(collection);
        var ctp = BuildCommandTypeProvider();
        var provider = collection.BuildServiceProvider();

        var configurationProvider = provider.GetRequiredService<INodeConfigurationProvider>();
        var configuration = configurationProvider.GetConfiguration();
        var listener = new TcpListener(IPAddress.Parse(configuration.Address), configuration.Port);
        listener.Start();
        while (true)
        {
            var client = listener.AcceptTcpClient();
            var stream = client.GetStream();
            var command = stream.AcceptCommand(ctp);
            var handler = provider.GetRequiredService<ICommandHandler>();
            handler.Accept(command, stream);
            stream.Close();
            client.Close();
        }
    }

    private static void ConfigureServices(ServiceCollection collection)
    {
        collection.AddSingleton<INodeConfigurationProvider, NodeConfigurationProvider>();
        collection.AddSingleton<INodeRepository, LocalNodeRepository>();
        collection.AddScoped<ICommandHandler, CommandHandler>();
    }

    private static CommandTypeProvider BuildCommandTypeProvider()
    {
        var ctp = new CommandTypeProvider();
        ctp.RegisterCommand<DeleteBlocksCommand>()
            .RegisterCommand<GetNodeConfigurationCommand>()
            .RegisterCommand<ReadBlockCommand>()
            .RegisterCommand<WriteBlockCommand>()
            .RegisterCommand<ExtractBlockCommand>();
        return ctp;
    }
}