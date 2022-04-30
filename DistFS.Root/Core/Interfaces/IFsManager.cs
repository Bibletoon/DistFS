namespace DistFS.Core;

public interface IFsManager
{
    void WriteFile(string localPath, string remotePath);
    void ReadFile(string remotePath, string localPath);
    void RemoveFile(string remotePath);
}