using Microsoft.Extensions.DependencyInjection;

namespace DistFS.RootClient.Commands;

public class ExecuteCommand : Command
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CommandTypeProvider _commandTypeProvider;

    public ExecuteCommand(IServiceProvider serviceProvider, CommandTypeProvider commandTypeProvider)
    {
        _serviceProvider = serviceProvider;
        _commandTypeProvider = commandTypeProvider;
    }

    public new static string CommandName => "execute-commands";
    
    public override async Task ExecuteAsync(string[] args)
    {
        foreach (var commandRequest in File.ReadLines(args[0]))
        {
            var commandName = string.Join("", commandRequest.Split(' ').First().Skip(1));
            var commandArguments = commandRequest.Split(' ').Skip(1).ToArray();

            if (commandName == "exit")
                return;
            
            var commandType = _commandTypeProvider.GetCommandType(commandName);
            var command = (Command)_serviceProvider.GetRequiredService(commandType);
            await command.ExecuteAsync(commandArguments);
        }
    }
}