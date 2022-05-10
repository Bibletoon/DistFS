using DistFs.Tcp.Common.Commands;

namespace DistFs.Tcp.Common;

public class CommandTypeProvider
{
    private Dictionary<string, Type> _types = new Dictionary<string, Type>();

    public CommandTypeProvider RegisterCommand<T>() where T : Command
    {
        _types.Add(typeof(T).Name, typeof(T));
        return this;
    }

    public Type GetCommandType(string typeName)
    {
        return _types[typeName];
    }
}