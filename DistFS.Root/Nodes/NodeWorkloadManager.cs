﻿using DistFS.Core;
using DistFS.Infrastructure;
using DistFS.Infrastructure.Database;
using DistFS.Models;
using DistFS.Nodes.Clients.Interfaces;
using DistFS.Tools.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DistFS.Nodes;

public class NodeWorkloadManager : INodeWorkloadManager
{
    private class NodeMemoryInfo
    {
        public long FreeSpace { get; set; }
        public long Size { get; }

        public NodeMemoryInfo(long freeSpace, long size)
        {
            FreeSpace = freeSpace;
            Size = size;
        }
    }
    
    private readonly INodeContext _nodeContext;
    private readonly IBlockContext _blockContext;
    private readonly INodeFileClient _fileClient;

    public NodeWorkloadManager(INodeFileClient fileClient, INodeContext nodeContext, IBlockContext blockContext)
    {
        _fileClient = fileClient;
        _nodeContext = nodeContext;
        _blockContext = blockContext;
    }

    public void RebalanceNodes()
    {
        var nodesMemory = new Dictionary<NodeInfo, NodeMemoryInfo>();
        var enumeratedBlocks = _blockContext.EnumerateBlocks();
        foreach (var node in _nodeContext.Nodes.ToList())
        {
            nodesMemory[node] = new NodeMemoryInfo(node.Size, node.Size);
        }

        var blocksToMove = new List<(NodeInfo, BlockInfo)>();
        foreach (var block in enumeratedBlocks.Select(b => b.Block))
        {
            var node = GetBestNode(block.Size, block.NodeId, nodesMemory);
            nodesMemory[node].FreeSpace -= block.Size;
            if (node.Id != block.NodeId)
                blocksToMove.Add((node, block));
        }

        var blocksToWrite = new List<(NodeInfo, BlockInfo, byte[])>();
        foreach (var (node, block) in blocksToMove)
        {
            var currentNode = _nodeContext.Nodes.Find(block.NodeId);
            var blockData = _fileClient.ReadBlock(currentNode, block.Name);
            _fileClient.DeleteBlock(currentNode, block.Name);
            blocksToWrite.Add((node, block, blockData));
        }

        foreach (var (node, block, data) in blocksToWrite)
        {
            _fileClient.WriteBlock(node, block.Name, data);
            _blockContext.UpdateBlockNode(block.Name, node.Id);
        }
    }

    private NodeInfo GetBestNode(long size, Guid priorityNodeId, Dictionary<NodeInfo, NodeMemoryInfo> nodeMemoryInfos)
    {
        return nodeMemoryInfos
            .Where(n => n.Value.FreeSpace >= size)
            .MaxBy(n =>
                n.Key.Id == priorityNodeId
                    ? (double)(n.Value.FreeSpace + 1) / n.Value.Size
                    : (double)n.Value.FreeSpace / n.Value.Size).Key;
    }
    
    public void CleanNode(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        
        var node = _nodeContext.Nodes.First(n => n.Name == name);

        var blocks = _blockContext.Blocks.Where(b => b.NodeId == node.Id);
        foreach (var block in blocks)
        {
            var blockData = _fileClient.ReadBlock(node, block.Name);
            _fileClient.DeleteBlock(node, block.Name);
            var newNode = _nodeContext.GetBestNode(blockData.Length, block.NodeId);
            _fileClient.WriteBlock(newNode, block.Name, blockData);
            _blockContext.UpdateBlockNode(block.Name, newNode.Id);
        }
    }
}