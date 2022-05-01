using DistFS.Models;

namespace DistFS.Core;

public interface IFileInfoManager
{
    RemoteFileInfo GetFileInfo(string remotePath);
    void AddFile(RemoteFileInfo fileInfo);
    void RemoveFile(RemoteFileInfo fileInfo);
}