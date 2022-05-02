using DistFS.Nodes;

namespace DistFS.RootClient.Commands;

public class BalanceNodesCommand : Command
{
    private readonly INodeWorkloadManager _manager;

    public BalanceNodesCommand(INodeWorkloadManager manager)
    {
        _manager = manager;
    }

    public new static string CommandName => "balance-nodes";
    public override void Execute(string[] args)
    {
        _manager.RebalanceNodes();
    }
}