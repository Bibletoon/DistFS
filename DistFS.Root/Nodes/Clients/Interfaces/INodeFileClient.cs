﻿using DistFS.Models;

namespace DistFS.Nodes.Clients.Interfaces;

public interface INodeFileClient
{
    void WriteBlock(NodeInfo node, string blockName, byte[] block);
    byte[] ReadBlock(NodeInfo node, string blockName);
    void DeleteBlocks(NodeInfo node, List<string> blocks);
    byte[] ExtractBlock(NodeInfo node, string blockName);
}