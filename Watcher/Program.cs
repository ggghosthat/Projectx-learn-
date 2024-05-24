using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Watcher;

internal class Program
{
    private static EndPoint _watchEndpoint;
    private static Socket _watchSocket;

    static void Main(string[] args)
    {
        _watchSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _watchEndpoint = new IPEndPoint(IPAddress.Any, 9050);
        _watchSocket.Bind(_watchEndpoint);

        byte[] data = new byte[1024];
        Console.WriteLine("Watching proccess is started: ");
        while (true)
        {
            int recv = _watchSocket.ReceiveFrom(data, ref _watchEndpoint);

            string message = Encoding.ASCII.GetString(data, 0, recv);
            Console.WriteLine(message);
        }
    }
}
