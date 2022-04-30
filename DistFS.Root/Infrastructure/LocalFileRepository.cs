namespace DistFS.Infrastructure;

public class LocalFileRepository : IFileRepository
{
    public byte[] ReadFile(string path)
    {
        return File.ReadAllBytes(path);
    }

    public void WriteFile(string path, byte[] content)
    {
        File.WriteAllBytes(path, content);
    }
}