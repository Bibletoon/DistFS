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
    public override async Task ExecuteAsync(string[] args)
    {
        await _manager.RebalanceNodesAsync();
    }
}