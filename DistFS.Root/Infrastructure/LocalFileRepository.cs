namespace DistFS.Infrastructure;

public class LocalFileRepository : IFileRepository
{
    public Stream OpenFile(string path)
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

    public async Task AppendFileContentAsync(string path, byte[] content)
    {
        var fs = File.Open(path, FileMode.Append);
        await fs.WriteAsync(content);
        fs.Close();
    }
}