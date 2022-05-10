namespace DistFs.Tcp.Common.Commands;

public abstract class Command
{
    public abstract void AcceptHandler(ICommandHandler handler, Stream stream);
}