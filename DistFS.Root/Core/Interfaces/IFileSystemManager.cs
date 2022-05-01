namespace DistFS.Core;

public interface IFileSystemManager
{
    void WriteFile(string localPath, string remotePath);
    void ReadFile(string remotePath, string localPath);
    void RemoveFile(string remotePath);
}