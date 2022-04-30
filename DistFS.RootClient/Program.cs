// See https://aka.ms/new-console-template for more information

using DistFS.Models;
using DistFS.Nodes.Clients.Tcp;
using DistFS.RootClient;
using Microsoft.Extensions.DependencyInjection;

public static class Program
{
    public static void Main(string[] args)
    {
        var runner = new RootRunner();
        runner.Run();
    }
}