// See https://aka.ms/new-console-template for more information

namespace DistFS.RootClient;

public static class Program
{
    public static void Main(string[] args)
    {
        var runner = new RootRunner();
        runner.Run();
    }
}