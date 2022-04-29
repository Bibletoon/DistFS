namespace DistFs.Tcp.Common;

public class WriteBlockCommand : Command
{
    public string BlockName { get; init; }
    public byte[] Block { get; init; }

    public WriteBlockCommand(string blockName, byte[] block)
    {
        BlockName = blockName;
        Block = block;
    }

    public override void AcceptHandler(ICommandHandler handler, Stream stream)
    {
        handler.Handle(this, stream);
    }
}