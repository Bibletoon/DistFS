namespace DistFs.Tcp.Common.Dto;

public class NodeConfigurationDto
{
    public Guid Id { get; init; }
    public long Size { get; init; }
    public long FreeSize { get; init; }

    public NodeConfigurationDto(Guid id, long size, long freeSize)
    {
        Id = id;
        Size = size;
        FreeSize = freeSize;
    }
}