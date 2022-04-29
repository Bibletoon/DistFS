namespace DistFs.Tcp.Common;

public class ReadBlockCommand : Command
{
    public string BlockName { get; init; }

    public ReadBlockCommand(string blockName)
    {
        BlockName = blockName;
    }

    public override void AcceptHandler(ICommandHandler handler, Stream stream)
    {
        handler.Handle(this, stream);
    }
}