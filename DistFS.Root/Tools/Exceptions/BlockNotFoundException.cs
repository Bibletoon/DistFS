namespace DistFS.Tools.Exceptions;

public class BlockNotFoundException : Exception
{
    public BlockNotFoundException() { }

    public BlockNotFoundException(string message)
        : base(message)
    { }
}