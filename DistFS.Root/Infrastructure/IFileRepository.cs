namespace DistFS.Infrastructure;

public interface IFileRepository
{
    Stream ReadFile(string path);
    FileInfo GetFileInfo(string path);
    void WriteFile(string path, ReadOnlySpan<byte> content);
}