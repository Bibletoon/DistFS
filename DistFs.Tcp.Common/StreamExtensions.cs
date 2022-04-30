﻿using System.Net.Sockets;
using System.Text;
using System.Text.Unicode;
using Newtonsoft.Json;

namespace DistFs.Tcp.Common;

public static class StreamExtensions
{
    public static byte[] AcceptBytes(this Stream stream)
    {
        byte[] buffer = new byte[512];
        var bytesRead = 0;

        var headerRead = 0;
        while (headerRead < 4 && (bytesRead = stream.Read(buffer, headerRead, 4-headerRead)) > 0)
        {
            headerRead += bytesRead;
        }

        var bytesRemaining = BitConverter.ToInt32(buffer);

        var data = new List<byte>();
        while (bytesRemaining > 0 && (bytesRead = stream.Read(buffer, 0, Math.Min(buffer.Length, bytesRemaining))) != 0)
        {
            data.AddRange(buffer.Take(bytesRead));
            bytesRemaining -= bytesRead;
        }

        return data.ToArray();
    }

    public static void SendBytes(this Stream stream, byte[] array)
    {
        stream.Write(BitConverter.GetBytes(array.Length), 0, 4);
        stream.Write(array, 0, array.Length);
    }

    public static void SendCommand(this Stream stream, Command command)
    {
        var commandTypeName = command.GetType().Name;
        var commandTypeNameBytes = Encoding.UTF8.GetBytes(commandTypeName);
        stream.SendBytes(commandTypeNameBytes);
        var commandBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command));
        stream.SendBytes(commandBytes);
    }

    public static Command AcceptCommand(this Stream stream, CommandTypeProvider typeProvider)
    {
        var commandTypeNameBytes = stream.AcceptBytes();
        var commandTypeName = Encoding.UTF8.GetString(commandTypeNameBytes);
        var commandType = typeProvider.GetCommandType(commandTypeName);

        var commandBytes = stream.AcceptBytes();
        var command = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(commandBytes), commandType);
        return (Command)command;
    }

    public static T Accept<T>(this Stream stream)
    {
        var objectBytes = stream.AcceptBytes();
        var objectString = Encoding.UTF8.GetString(objectBytes);
        var obj = JsonConvert.DeserializeObject<T>(objectString);

        return obj;
    }

    public static void Send<T>(this Stream stream, T obj)
    {
        stream.SendBytes(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj)));
    }
}