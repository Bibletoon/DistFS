using DistFS.Models;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Infrastructure.Database;

public interface IFileInfoContext
{ 
    DbSet<RemoteFileInfo> RemoteFiles { get; set; }
    int SaveChanges();
    
    RemoteFileInfo GetFileInfo(string remotePath)
    {
        var fileInfo = RemoteFiles.Include(df => df.Blocks).FirstOrDefault();
        if (fileInfo is null)
            throw new FileNotFoundException($"File at path {remotePath} not found");

        return fileInfo;
    }
}