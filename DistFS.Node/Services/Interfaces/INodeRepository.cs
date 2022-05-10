namespace DistFS.Node.Services.Interfaces;

public interface INodeRepository
{
    FileInfo GetFileInfo(string fileName);
    long WriteFile(string fileName, byte[] data);
    byte[] ReadFile(string fileName);
    long RemoveFile(string fileName);
}