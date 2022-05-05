namespace DistFs.Tcp.Common;

public class DeleteBlocksCommand : Command
{
    public List<string> Blocks { get; init; }
 
    public DeleteBlocksCommand(List<string> blocks)
    {
        Blocks = blocks;
    }

    public override async Task AcceptHandler(ICommandHandler handler, Stream stream)
    {
        await handler.Handle(this, stream);
    }
}