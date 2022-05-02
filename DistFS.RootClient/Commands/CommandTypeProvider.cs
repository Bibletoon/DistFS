namespace DistFS.RootClient.Commands;

public class CommandTypeProvider
{
    private readonly Dictionary<string, Type> _types;

    public CommandTypeProvider()
    {
        _types = new Dictionary<string, Type>();
    }

    public CommandTypeProvider RegisterCommand<T>() where T : Command
    {
        var name = typeof(T).GetProperty("CommandName").GetValue(null).ToString();
        _types[name] = typeof(T);
        return this;
    }

    public Type GetCommandType(string command)
    {
        return _types[command];
    }
}