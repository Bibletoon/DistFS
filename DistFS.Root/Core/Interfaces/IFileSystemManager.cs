namespace DistFS.Core.Interfaces;

public interface IFileSystemManager
{
    Task WriteFileAsync(string localPath, string remotePath);
    Task ReadFileAsync(string remotePath, string localPath);
    Task RemoveFileAsync(string remotePath);
}