namespace DistFs.Tcp.Common.Commands;

public class GetNodeConfigurationCommand : Command
{
    public override void AcceptHandler(ICommandHandler handler, Stream stream)
    {
        handler.Handle(this, stream);
    }
}