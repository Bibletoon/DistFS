using DistFS.Nodes;

namespace DistFS.RootClient.Commands;

public class CleanNodeCommand : Command
{
    private INodeWorkloadManager _manager;

    public CleanNodeCommand(INodeWorkloadManager manager)
    {
        _manager = manager;
    }

    public new static string CommandName => "clean-node";

    public override void Execute(string[] args)
    {
        if (args.Length != 1)
            throw new ArgumentException("Wrong arguments count");
        
        _manager.CleanNode(args[0]);
    }
}