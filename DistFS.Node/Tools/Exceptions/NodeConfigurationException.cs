namespace DistFS.Node.Tools.Exceptions;

public class NodeConfigurationException : Exception
{
    public NodeConfigurationException()
    { }

    public NodeConfigurationException(string message)
        : base(message)
    { }
}