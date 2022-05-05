using DistFS.Core.Interfaces;

namespace DistFS.RootClient.Commands;

public class RemoveFileCommand : Command
{
    private readonly IFileSystemManager _fileSystemManager;

    public RemoveFileCommand(IFileSystemManager fileSystemManager)
    {
        _fileSystemManager = fileSystemManager;
    }

    public new static string CommandName => "remove-file";
    public override async Task ExecuteAsync(string[] args)
    {
        if (args.Length != 1)
            throw new ArgumentException("Wrong arguments count");
        
        await _fileSystemManager.RemoveFileAsync(args[0]);
    }
}