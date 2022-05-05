namespace DistFS.RootClient.Commands;

public abstract class Command
{
    public static string CommandName;
    public abstract Task ExecuteAsync(string[] args);
}