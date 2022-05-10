using DistFS.Node.Services.Interfaces;
using DistFs.Tcp.Common;
using DistFs.Tcp.Common.Commands;
using DistFs.Tcp.Common.Dto;

namespace DistFS.Node.Services;

public class CommandHandler : ICommandHandler
{
    private readonly INodeRepository _repository;
    private readonly INodeConfigurationProvider _configurationProvider;

    public CommandHandler(INodeRepository repository, INodeConfigurationProvider configurationProvider)
    {
        _repository = repository;
        _configurationProvider = configurationProvider;
    }

    public void Accept(Command command, Stream stream)
    {
        Console.WriteLine($"Accepting {command.GetType().Name}");
        command.AcceptHandler(this, stream);
    }

    public void Handle(DeleteBlocksCommand command, Stream stream)
    {
        long deletedFileLength = 0;
        foreach (var block in command.Blocks)
        {
            deletedFileLength += _repository.RemoveFile(block);
        }
        _configurationProvider.IncreaseFreeSpace(deletedFileLength);
        stream.SendBytes(BitConverter.GetBytes(_configurationProvider.GetFreeSpace()));
    }

    public void Handle(GetNodeConfigurationCommand command, Stream stream)
    {
        var configuration = _configurationProvider.GetConfiguration();
        var configurationDto = new NodeConfigurationDto(configuration.Id, configuration.Size, configuration.FreeSpace);
        stream.Send(configurationDto);
    }

    public void Handle(ReadBlockCommand command, Stream stream)
    {
        var block = _repository.ReadFile(command.BlockName);
        stream.SendBytes(block);
    }

    public void Handle(WriteBlockCommand command, Stream stream)
    {
        var writtenFileLength = _repository.WriteFile(command.BlockName, command.Block);
        _configurationProvider.DecreaseFreeSpace(writtenFileLength);
        stream.SendBytes(BitConverter.GetBytes(_configurationProvider.GetFreeSpace()));
    }

    public void Handle(ExtractBlockCommand command, Stream stream)
    {
        var block = _repository.ReadFile(command.BlockName);
        var deletedFileSize = _repository.RemoveFile(command.BlockName);
        _configurationProvider.IncreaseFreeSpace(deletedFileSize);
        stream.SendBytes(block);
        stream.SendBytes(BitConverter.GetBytes(_configurationProvider.GetFreeSpace()));
    }
}