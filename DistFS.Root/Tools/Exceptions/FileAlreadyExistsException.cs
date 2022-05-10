namespace DistFS.Tools.Exceptions;

public class FileAlreadyExistsException : Exception
{
    public FileAlreadyExistsException() { }
    public FileAlreadyExistsException(string message) : base(message) { }
    public FileAlreadyExistsException(string message, Exception exception) : base(message, exception) { }
}