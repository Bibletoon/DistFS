using DistFs.Tcp.Common;
using DistFs.Tcp.Common.Dto;
using DistFs.Tcp.Common.NodeAbstractions;

namespace DistFS.Node;

public class CommandHandler : ICommandHandler
{
    private readonly INodeRepository _repository;
    private readonly INodeConfigurationProvider _configurationProvider;

    public CommandHandler(INodeRepository repository, INodeConfigurationProvider configurationProvider)
    {
        _repository = repository;
        _configurationProvider = configurationProvider;
    }

    public async Task Accept(Command command, Stream stream)
    {
        Console.WriteLine($"Accepting {command.GetType().Name}");
        await command.AcceptHandler(this, stream);
    }

    public async Task Handle(DeleteBlocksCommand command, Stream stream)
    {
        long deletedFileLength = 0;
        foreach (var block in command.Blocks)
        {
            deletedFileLength += _repository.RemoveFile(block);
        }
        _configurationProvider.IncreaseFreeSpace(deletedFileLength);
        await stream.SendBytesAsync(BitConverter.GetBytes(_configurationProvider.GetFreeSpace()));
    }

    public async Task Handle(GetNodeConfigurationCommand command, Stream stream)
    {
        var configuration = _configurationProvider.GetConfiguration();
        var configurationDto = new NodeConfigurationDto(configuration.Id, configuration.Size, configuration.FreeSpace);
        await stream.SendAsync(configurationDto);
    }

    public async Task Handle(ReadBlockCommand command, Stream stream)
    {
        var block = _repository.ReadFile(command.BlockName);
        await stream.SendBytesAsync(block);
    }

    public async Task Handle(WriteBlockCommand command, Stream stream)
    {
        var writtenFileLength = _repository.WriteFile(command.BlockName, command.Block);
        _configurationProvider.DecreaseFreeSpace(writtenFileLength);
        await stream.SendBytesAsync(BitConverter.GetBytes(_configurationProvider.GetFreeSpace()));
    }

    public async Task Handle(ExtractBlockCommand command, Stream stream)
    {
        var block = _repository.ReadFile(command.BlockName);
        var deletedFileSize = _repository.RemoveFile(command.BlockName);
        _configurationProvider.IncreaseFreeSpace(deletedFileSize);
        await stream.SendBytesAsync(block);
        await stream.SendBytesAsync(BitConverter.GetBytes(_configurationProvider.GetFreeSpace()));
    }
}