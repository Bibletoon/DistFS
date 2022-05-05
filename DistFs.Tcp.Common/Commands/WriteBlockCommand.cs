using System.Text.Json.Serialization;

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

    public override async Task AcceptHandler(ICommandHandler handler, Stream stream)
    {
        await handler.Handle(this, stream);
    }
}