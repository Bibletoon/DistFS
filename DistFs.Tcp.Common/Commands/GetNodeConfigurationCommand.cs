namespace DistFs.Tcp.Common;

public class GetNodeConfigurationCommand : Command
{
    public override async Task AcceptHandler(ICommandHandler handler, Stream stream)
    {
        await handler.Handle(this, stream);
    }
}