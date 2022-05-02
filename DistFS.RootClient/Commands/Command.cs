namespace DistFS.RootClient.Commands;

public abstract class Command
{
    public static string CommandName;
    public abstract void Execute(string[] args);
}