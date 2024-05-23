using Projectx.Contracts.Logging;
using Projectx.Entity.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ProjectxAPI.Streaming;

public class Streamer : IStreamer, IDisposable
{
    private readonly ILoggerManager _loggerManager;
    private IPEndPoint _streamEndpoint;
    private Socket _streamSocket;

    public Streamer()
    {
    }

    public void Start(string streamerIp, int streamerPort)
    {
        var host = Dns.GetHostEntry(streamerIp);
        var ipAddress = host.AddressList[0];        
        _streamEndpoint = new(ipAddress, streamerPort);

        _streamSocket = new (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _streamSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
        _streamSocket.Bind(_streamEndpoint);
    }

    public async Task Stream(Message message)
    {
        string text = JsonSerializer.Serialize(message);
        byte[] sendBytes = Encoding.ASCII.GetBytes(text);
        var remoteEP = new IPEndPoint(IPAddress.Any, 0);
        await _streamSocket.SendToAsync(sendBytes, remoteEP);
    }

    public void Dispose()
    {
        _streamSocket.Shutdown(SocketShutdown.Send);
        _streamSocket.Close();
    }
}