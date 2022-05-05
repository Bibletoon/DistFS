namespace DistFs.Tcp.Common;

public abstract class Command
{
    public abstract Task AcceptHandler(ICommandHandler handler, Stream stream);
}