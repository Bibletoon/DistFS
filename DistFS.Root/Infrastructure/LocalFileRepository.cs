namespace DistFS.Infrastructure;

public class LocalFileRepository : IFileRepository
{
    public Stream ReadFile(string path)
    {
        return File.Open(path, FileMode.Open);
    }

    public FileInfo GetFileInfo(string path)
    {
        return new FileInfo(path);
    }

    public void WriteFile(string path, ReadOnlySpan<byte> content)
    {
        var fs = File.Open(path, FileMode.Create);
        fs.Write(content);
        fs.Close();
    }
}