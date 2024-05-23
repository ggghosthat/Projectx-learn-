using Projectx.Entity.Models;

namespace ProjectxAPI.Streaming;

public interface IStreamer
{
    public void Start(string streamerIp, int streamerPort);
    public Task Stream(Message message);
}