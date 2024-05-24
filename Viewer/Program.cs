using Projectx.Entity.Models;
using System.Net;
using System.Net.Http.Json;

namespace Viewer;

internal class Program
{
    private const string APP_PATH = "http://localhost:5281";

    static async Task Main(string[] args)
    {
        await ViewHistory();
    }

    private async static Task ViewHistory()
    {
        using (var client = new HttpClient())
        {
            var responseMessages = await client.GetFromJsonAsync<IEnumerable<Message>>(APP_PATH + "/api/Message/LastHistory");

            foreach (var message in responseMessages)
                Console.WriteLine($"MessageId: {message.MessageId}\n ClientId: {message.ClientId}\n Created: {message.Created}\n Content: {message.Content}\n");
        }
    }
}
