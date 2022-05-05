using System.Net;
using System.Net.Sockets;
using DistFS.Node;
using DistFs.Tcp.Common;

var runner = new TcpNodeRunner();

await runner.RunAsync();