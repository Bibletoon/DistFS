﻿namespace DistFs.Tcp.Common.NodeAbstractions;

public class NodeConfigurationInfo
{
    public Guid Id { get; }
    public long Size { get; }
    public long FreeSpace { get; set; }
    public string Address { get; set; }
    public int Port { get; set; }
    
    public NodeConfigurationInfo(Guid id, int size, int freeSpace, string address, int port)
    {
        Id = id;
        Size = size;
        FreeSpace = freeSpace;
        Address = address;
        Port = port;
    }
}