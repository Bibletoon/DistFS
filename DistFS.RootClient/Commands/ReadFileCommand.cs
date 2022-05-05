using DistFS.Core.Interfaces;

namespace DistFS.RootClient.Commands;

public class ReadFileCommand : Command
{
    private readonly IFileSystemManager _fileSystemManager;

    public ReadFileCommand(IFileSystemManager fileSystemManager)
    {
        _fileSystemManager = fileSystemManager;
    }

    public new static string CommandName => "read-file";
    public override async Task ExecuteAsync(string[] args)
    {
        if (args.Length != 2)
            throw new ArgumentException("Wrong arguments count");
        
        await _fileSystemManager.ReadFileAsync(args[0], args[1]);
    }
}