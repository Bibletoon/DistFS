namespace DistFs.Tcp.Common.NodeAbstractions;

public interface INodeRepository
{
    FileInfo GetFileInfo(string fileName);
    public void WriteFile(string fileName, byte[] data);
    public byte[] ReadFile(string fileName);
    public void RemoveFile(string fileName);
}