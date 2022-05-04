﻿using DistFS.Models;

namespace DistFS.Nodes.Clients.Interfaces;

public interface INodeFileClient
{
    void WriteBlock(NodeInfo node, string blockName, ReadOnlySpan<byte> block);
    ReadOnlySpan<byte> ReadBlock(NodeInfo node, string blockName);
    void DeleteBlock(NodeInfo node, string blockName);
}