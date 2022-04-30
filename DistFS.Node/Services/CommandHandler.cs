using System.Text;
using DistFs.Tcp.Common;
using DistFs.Tcp.Common.Dto;
using DistFs.Tcp.Common.NodeAbstractions;
using Newtonsoft.Json;

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

    public void Accept(Command command, Stream stream)
    {
        command.AcceptHandler(this, stream);
    }

    public void Handle(DeleteBlockCommand command, Stream stream)
    {
        _repository.RemoveFile(command.BlockName);
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
        _repository.WriteFile(command.BlockName, command.Block);
        stream.SendBytes(BitConverter.GetBytes(_configurationProvider.GetFreeSpace()));
    }
}