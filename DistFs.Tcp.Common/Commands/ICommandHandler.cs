using System.Reflection.Metadata;

namespace DistFs.Tcp.Common;

public interface ICommandHandler
{
    Task Accept(Command command, Stream stream);
    Task Handle(DeleteBlocksCommand command, Stream stream);
    Task Handle(GetNodeConfigurationCommand command, Stream stream);
    Task Handle(ReadBlockCommand command, Stream stream);
    Task Handle(WriteBlockCommand command, Stream stream);
    Task Handle(ExtractBlockCommand command, Stream stream);
}