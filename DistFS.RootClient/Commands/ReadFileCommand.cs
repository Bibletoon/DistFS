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
    public override void Execute(string[] args)
    {
        if (args.Length != 2)
            throw new ArgumentException("Wrong arguments count");
        
        _fileSystemManager.ReadFile(args[0], args[1]);
    }
}