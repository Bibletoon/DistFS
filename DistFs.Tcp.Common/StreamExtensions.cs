using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System;

namespace DistFs.Tcp.Common;

public static class StreamExtensions
{
    public static async Task<byte[]> AcceptBytesAsync(this Stream stream)
    {
        byte[] buffer = new byte[512];
        var bytesRead = 0;

        var headerRead = 0;
        while (headerRead < 4 && (bytesRead = await stream.ReadAsync(buffer.AsMemory(headerRead, 4 - headerRead))) > 0)
        {
            headerRead += bytesRead;
        }

        var bytesRemaining = BitConverter.ToInt32(buffer);

        var data = new List<byte>();
        while (bytesRemaining > 0 && (bytesRead = await stream.ReadAsync(buffer.AsMemory(0, Math.Min(buffer.Length, bytesRemaining)))) != 0)
        {
            data.AddRange(buffer.Take(bytesRead));
            bytesRemaining -= bytesRead;
        }

        return data.ToArray();
    }

    public static async Task SendBytesAsync(this Stream stream, byte[] array)
    {
        await stream.WriteAsync(BitConverter.GetBytes(array.Length).AsMemory(0, 4));
        await stream.WriteAsync(array);
    }

    public static async Task SendCommandAsync(this Stream stream, Command command)
    {
        var commandTypeName = command.GetType().Name;
        var commandTypeNameBytes = Encoding.UTF8.GetBytes(commandTypeName);
        await stream.SendBytesAsync(commandTypeNameBytes);
        var commandBytes = JsonSerializer.SerializeToUtf8Bytes(command, command.GetType());
        await stream.SendBytesAsync(commandBytes);
    }

    public static async Task<Command> AcceptCommandAsync(this Stream stream, CommandTypeProvider typeProvider)
    {
        var commandTypeNameBytes = await stream.AcceptBytesAsync();
        var commandTypeName = Encoding.UTF8.GetString(commandTypeNameBytes);
        var commandType = typeProvider.GetCommandType(commandTypeName);

        var commandBytes = await stream.AcceptBytesAsync();
        var command = JsonSerializer.Deserialize(commandBytes, commandType);
        return (Command)command;
    }

    public static async Task<T> AcceptAsync<T>(this Stream stream)
    {
        var objectBytes = await stream.AcceptBytesAsync();
        var obj = JsonSerializer.Deserialize<T>(objectBytes);

        return obj;
    }

    public static async Task SendAsync<T>(this Stream stream, T obj)
    {
        await stream.SendBytesAsync(JsonSerializer.SerializeToUtf8Bytes(obj, obj.GetType()));
    }
}