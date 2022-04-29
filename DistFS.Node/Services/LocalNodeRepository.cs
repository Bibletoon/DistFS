using DistFs.Tcp.Common.NodeAbstractions;

namespace DistFS.Node;

public class LocalNodeRepository : INodeRepository
{
    private readonly INodeConfigurationProvider _configurationProvider;
    private readonly string _folderName;

    public LocalNodeRepository(INodeConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
        _folderName = configurationProvider.GetConfiguration().Id.ToString();
        if (!Directory.Exists(_folderName))
            Directory.CreateDirectory(_folderName);
    }

    public FileInfo GetFileInfo(string fileName)
    {
        return new FileInfo(Path.Combine(_folderName, fileName));
    }

    public void WriteFile(string fileName, byte[] data)
    {
        File.WriteAllBytes(Path.Combine(_folderName, fileName), data);
        _configurationProvider.DecreaseFreeSpace(data.Length);
    }

    public byte[] ReadFile(string fileName)
    {
        return File.ReadAllBytes(Path.Combine(_folderName, fileName));
    }

    public void RemoveFile(string fileName)
    {
        var length = new FileInfo(Path.Combine(_folderName, fileName)).Length;
        File.Delete(Path.Combine(_folderName, fileName));
        _configurationProvider.IncreaseFreeSpace(length);
    }
}