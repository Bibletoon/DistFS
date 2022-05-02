using Microsoft.Extensions.DependencyInjection;

namespace DistFS.RootClient.Commands;

public class ExecuteCommand : Command
{
    private IServiceProvider _serviceProvider;
    private CommandTypeProvider _commandTypeProvider;

    public ExecuteCommand(IServiceProvider serviceProvider, CommandTypeProvider commandTypeProvider)
    {
        _serviceProvider = serviceProvider;
        _commandTypeProvider = commandTypeProvider;
    }

    public new static string CommandName => "execute-commands";
    
    public override void Execute(string[] args)
    {
        var commandRequest = Console.ReadLine();
        var commandName = string.Join("", commandRequest.Split(' ').First().Skip(1));
        var commandArguments = commandRequest.Split(' ').Skip(1).ToArray();

        if (commandName == "exit")
            return;
            
        var commandType = _commandTypeProvider.GetCommandType(commandName);
        var command = (Command)_serviceProvider.GetRequiredService(commandType);
        command.Execute(commandArguments);
    }
}