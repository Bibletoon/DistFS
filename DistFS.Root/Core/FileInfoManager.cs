using DistFS.Infrastructure;
using DistFS.Models;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Core;

public class FileInfoManager : IFileInfoManager
{
    private RootDbContext _context;

    public FileInfoManager(RootDbContext context)
    {
        _context = context;
    }

    public RemoteFileInfo GetFileInfo(string remotePath)
    {
        var fileInfo = _context.RemoteFiles.Include(df => df.Blocks).FirstOrDefault();
        if (fileInfo is null)
            throw new FileNotFoundException($"File at path {remotePath} not found");

        return fileInfo;
    }

    public void AddFile(RemoteFileInfo fileInfo)
    {
        _context.RemoteFiles.Add(fileInfo);
        _context.SaveChanges();
    }

    public void RemoveFile(RemoteFileInfo fileInfo)
    {
        _context.RemoteFiles.Remove(fileInfo);
    }
}