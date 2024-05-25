using Projectx.Entity.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Projectx.Client;

internal class Watcher
{
    private static EndPoint _watchEndpoint;
    private static Socket _watchSocket;

    public static List<Message> Messages { get; private set; } = new();
    public static event Action NotificationEvent;

    public static void Watch()
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
            Messages.Add(message);
            NotificationEvent.Invoke();
        }
    }
}