using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;
using Projectx.Entity.Models;

namespace Projectx.Watcher
{
    internal class Program
    {
        private static EndPoint _watchEndpoint;
        private static Socket _watchSocket;

        static void Main(string[] args)
        {
            _watchSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _watchEndpoint = new IPEndPoint(IPAddress.Any, 9050);
            _watchSocket.Connect(_watchEndpoint);

            byte[] data = new byte[4096];
            Console.WriteLine("Watching proccess is started: ");
            while (true)
            {
                int recv = _watchSocket.ReceiveFrom(data, ref _watchEndpoint);
                string json = Encoding.ASCII.GetString(data, 0, recv);
                var message = JsonSerializer.Deserialize<Message>(json);
                Console.WriteLine(message.ToString());
            }
        }
    }
}
