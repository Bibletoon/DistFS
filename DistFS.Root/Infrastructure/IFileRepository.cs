namespace DistFS.Infrastructure;

public interface IFileRepository
{
    Stream ReadFile(string path);
    FileInfo GetFileInfo(string path);
    void CreateFile(string path);
    void AppendFileContent(string path, byte[] content);
}