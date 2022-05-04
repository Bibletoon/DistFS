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

    public void CreateFile(string path)
    {
        File.Create(path).Close();
    }

    public void AppendFileContent(string path, ReadOnlySpan<byte> content)
    {
        var fs = File.Open(path, FileMode.Append);
        fs.Write(content);
        fs.Close();
    }
}