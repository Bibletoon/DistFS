using System.Text.Json;
using DistFs.Tcp.Common.NodeAbstractions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DistFS.Node;

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
        
        _configurationInfo = JsonConvert.DeserializeObject<NodeConfigurationInfo>(File.ReadAllText(ConfigFileName));
    }
    
    public NodeConfigurationInfo GetConfiguration()
    {
        return _configurationInfo;
    }

    public void IncreaseFreeSpace(long space)
    {
        _configurationInfo.FreeSpace += space;
        File.WriteAllText(ConfigFileName, JsonConvert.SerializeObject(_configurationInfo));
    }

    public void DecreaseFreeSpace(long space)
    {
        _configurationInfo.FreeSpace -= space;
        File.WriteAllText(ConfigFileName, JsonConvert.SerializeObject(_configurationInfo));
    }

    public long GetFreeSpace()
    {
        return _configurationInfo.FreeSpace;
    }
}