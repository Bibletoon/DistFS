namespace DistFS.Models;

public class RemoteFileInfo
{
    public string RemotePath { get; private init; }
    public List<BlockInfo> Blocks { get; private init; }

    private RemoteFileInfo() { }
    
    public RemoteFileInfo(string remotePath, List<BlockInfo> blocks)
    {
        RemotePath = remotePath;
        Blocks = blocks;
    }
}