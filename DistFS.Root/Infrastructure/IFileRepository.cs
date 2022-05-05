namespace DistFS.Infrastructure;

public interface IFileRepository
{
    Stream OpenFile(string path);
    FileInfo GetFileInfo(string path);
    void CreateFile(string path);
    Task AppendFileContentAsync(string path, byte[] content);
}