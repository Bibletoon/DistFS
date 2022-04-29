using Newtonsoft.Json;

namespace DistFs.Tcp.Common;

public class DeleteBlockCommand : Command
{
    public string BlockName { get; init; }
 
    public DeleteBlockCommand(string blockName)
    {
        BlockName = blockName;
    }

    public override void AcceptHandler(ICommandHandler handler, Stream stream)
    {
        handler.Handle(this, stream);
    }
}