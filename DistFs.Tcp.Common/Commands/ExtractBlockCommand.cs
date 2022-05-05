namespace DistFs.Tcp.Common;

public class ExtractBlockCommand : Command
{
    public ExtractBlockCommand(string blockName)
    {
        BlockName = blockName;
    }

    public string BlockName { get; init; }

    public override async Task AcceptHandler(ICommandHandler handler, Stream stream)
    {
        await handler.Handle(this, stream);
    }
}