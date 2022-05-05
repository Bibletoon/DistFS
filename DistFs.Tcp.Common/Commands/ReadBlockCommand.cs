namespace DistFs.Tcp.Common;

public class ReadBlockCommand : Command
{
    public string BlockName { get; init; }

    public ReadBlockCommand(string blockName)
    {
        BlockName = blockName;
    }

    public override async Task AcceptHandler(ICommandHandler handler, Stream stream)
    {
        await handler.Handle(this, stream);
    }
}