namespace DistFs.Tcp.Common.Commands;

public class DeleteBlocksCommand : Command
{
    public List<string> Blocks { get; init; }
 
    public DeleteBlocksCommand(List<string> blocks)
    {
        Blocks = blocks;
    }

    public override void AcceptHandler(ICommandHandler handler, Stream stream)
    {
        handler.Handle(this, stream);
    }
}