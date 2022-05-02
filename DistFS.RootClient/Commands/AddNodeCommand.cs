using DistFS.Core.Interfaces;

namespace DistFS.RootClient.Commands;

public class AddNodeCommand : Command
{
    private readonly INodeManager _nodeManager;

    public AddNodeCommand(INodeManager nodeManager)
    {
        _nodeManager = nodeManager;
    }

    public new static string CommandName => "add-node";

    public override void Execute(string[] args)
    {
        if (args.Length != 2)
            throw new ArgumentException("Wrong command arguments count");

        var ip = args[1].Split(':')[0];
        var port = int.Parse(args[1].Split(':')[1]);
        _nodeManager.RegisterNode(args[0], ip, port);
    }
}