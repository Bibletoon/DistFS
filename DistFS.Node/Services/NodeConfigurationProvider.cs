using System.Text.Json;
using DistFS.Node.Services.Interfaces;
using DistFS.Node.Tools.Exceptions;

namespace DistFS.Node.Services;

public class NodeConfigurationProvider : INodeConfigurationProvider
{
    private const string ConfigFileName = "nodeConfig.json";
    private readonly NodeConfigurationInfo _configurationInfo;

    public NodeConfigurationProvider()
    {
        if (!File.Exists(ConfigFileName))
        {
            throw new FileNotFoundException("Config file not found");
        }
        
        _configurationInfo = 
            JsonSerializer.Deserialize<NodeConfigurationInfo>(File.ReadAllText(ConfigFileName))
            ?? throw new NodeConfigurationException("Wrong configuration format");
    }
    
    public NodeConfigurationInfo GetConfiguration()
    {
        return _configurationInfo;
    }

    public void IncreaseFreeSpace(long space)
    {
        _configurationInfo.FreeSpace += space;
        File.WriteAllText(ConfigFileName, JsonSerializer.Serialize(_configurationInfo));
    }

    public void DecreaseFreeSpace(long space)
    {
        _configurationInfo.FreeSpace -= space;
        File.WriteAllText(ConfigFileName, JsonSerializer.Serialize(_configurationInfo));
    }

    public long GetFreeSpace()
    {
        return _configurationInfo.FreeSpace;
    }
}