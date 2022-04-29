using System.Reflection.Metadata;

namespace DistFs.Tcp.Common;

public interface ICommandHandler
{
    void Accept(Command command, Stream stream);
    void Handle(DeleteBlockCommand command, Stream stream);
    void Handle(GetNodeConfigurationCommand command, Stream stream);
    void Handle(ReadBlockCommand command, Stream stream);
    void Handle(WriteBlockCommand command, Stream stream);
}