using System.Buffers;
using System.Text;
using System.Text.Json;
using DistFs.Tcp.Common.Commands;

namespace DistFs.Tcp.Common;

public static class StreamExtensions
{
    public static byte[] AcceptBytes(this Stream stream)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(512);
        var bytesRead = 0;

        var headerRead = 0;
        while (headerRead < 4 && (bytesRead = stream.Read(buffer, headerRead, 4-headerRead)) > 0)
        {
            headerRead += bytesRead;
        }

        var bytesRemaining = BitConverter.ToInt32(buffer);
        var arrayIndex = 0;
        
        var data = new byte[bytesRemaining];
        while (bytesRemaining > 0 && (bytesRead = stream.Read(buffer, 0, Math.Min(buffer.Length, bytesRemaining))) != 0)
        {
            Array.Copy(buffer, 0, data, arrayIndex, bytesRead);
            bytesRemaining -= bytesRead;
            arrayIndex += bytesRead;
        }

        ArrayPool<byte>.Shared.Return(buffer);
        return data;
    }

    public static void SendBytes(this Stream stream, byte[] array)
    {
        stream.Write(BitConverter.GetBytes(array.Length), 0, 4);
        stream.Write(array);
    }

    public static void SendCommand(this Stream stream, Command command)
    {
        var commandTypeName = command.GetType().Name;
        var commandTypeNameBytes = Encoding.UTF8.GetBytes(commandTypeName);
        stream.SendBytes(commandTypeNameBytes);
        var commandBytes = JsonSerializer.SerializeToUtf8Bytes(command, command.GetType());
        stream.SendBytes(commandBytes);
    }

    public static Command AcceptCommand(this Stream stream, CommandTypeProvider typeProvider)
    {
        var commandTypeNameBytes = stream.AcceptBytes();
        var commandTypeName = Encoding.UTF8.GetString(commandTypeNameBytes);
        var commandType = typeProvider.GetCommandType(commandTypeName);

        var commandBytes = stream.AcceptBytes();
        var command = JsonSerializer.Deserialize(commandBytes, commandType);
        return (Command)command;
    }

    public static T Accept<T>(this Stream stream)
    {
        var objectBytes = stream.AcceptBytes();
        var obj = JsonSerializer.Deserialize<T>(objectBytes);

        return obj;
    }

    public static void Send<T>(this Stream stream, T obj)
    {
        stream.SendBytes(JsonSerializer.SerializeToUtf8Bytes(obj, obj.GetType()));
    }
}