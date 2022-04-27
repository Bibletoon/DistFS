namespace DistFS.Infrastructure;

public interface IFileRepository
{
    byte[] ReadFile(string path);
    void WriteFile(string path, byte[] content);
}