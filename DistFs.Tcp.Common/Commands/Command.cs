namespace DistFs.Tcp.Common;

public abstract class Command
{
    public abstract void AcceptHandler(ICommandHandler handler, Stream stream);
}