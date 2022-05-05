using DistFS.Models;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Infrastructure.Database;

public interface IFileInfoContext
{ 
    DbSet<RemoteFileInfo> RemoteFiles { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    async Task<RemoteFileInfo> GetFileInfoAsync(string remotePath)
    {
        var fileInfo = await RemoteFiles.Include(df => df.Blocks).FirstOrDefaultAsync();
        if (fileInfo is null)
            throw new FileNotFoundException($"File at path {remotePath} not found");

        return fileInfo;
    }
}