namespace DistFs.Tcp.Common;

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