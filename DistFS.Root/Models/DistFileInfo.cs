namespace DistFS.Models;

public class DistFileInfo
{
    public string RemotePath { get; init; }
    public List<BlockInfo> Blocks { get; init; }

    private DistFileInfo() { }
    
    public DistFileInfo(string remotePath, List<BlockInfo> blocks)
    {
        RemotePath = remotePath;
        Blocks = blocks;
    }
}