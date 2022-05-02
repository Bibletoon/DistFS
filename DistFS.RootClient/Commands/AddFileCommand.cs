using DistFS.Core.Interfaces;

namespace DistFS.RootClient.Commands;

public class AddFileCommand : Command
{
    private readonly IFileSystemManager _fileSystemManager;

    public AddFileCommand(IFileSystemManager fileSystemManager)
    {
        _fileSystemManager = fileSystemManager;
    }

    public new static string CommandName => "add-file";

    public override void Execute(string[] args)
    {
        if (args.Length != 2)
            throw new ArgumentException("Wrong arguments count");
        
        _fileSystemManager.WriteFile(args[0], args[1]);
    }
}