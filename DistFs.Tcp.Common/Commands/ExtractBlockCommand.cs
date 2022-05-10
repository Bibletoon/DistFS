namespace DistFs.Tcp.Common.Commands;

public class ExtractBlockCommand : Command
{
    public ExtractBlockCommand(string blockName)
    {
        BlockName = blockName;
    }

    public string BlockName { get; init; }

    public override void AcceptHandler(ICommandHandler handler, Stream stream)
    {
        handler.Handle(this, stream);
    }
}