using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Sender;

internal class Program
{
    private const string APP_PATH = "http://localhost:5281";
    private static HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        int clientId = await Login();

        await SendMessage(clientId);
    }

    private async static ValueTask<int> Login()
    {
        Console.WriteLine("Please enter the name for a new user:");
        string clientName = Console.ReadLine();
        int clientId;

        var registerModel = new
        {
            Name = clientName
        };

        using (var client = new HttpClient())
        {
            var response = await client.PostAsJsonAsync(APP_PATH + "/api/Gate/RegisterUser", registerModel);
            string json = await response.Content.ReadAsStringAsync();
            clientId = JsonSerializer.Deserialize<int>(json);
        }

        return clientId;
    }

    private async static Task SendMessage(int clientId)
    {
        Console.WriteLine("Please enter your message:");
        string content = Console.ReadLine();

        var messageModel = new
        {
            ClientId = clientId,
            Content = content
        };

        using (var client = new HttpClient())
        {
            var response = await client.PostAsJsonAsync(APP_PATH + "/api/Message/SendMessage", messageModel);
            
            if (response.StatusCode == HttpStatusCode.OK)
                Console.WriteLine("Message sent successfully.");
            else
                Console.WriteLine("Something went wrong.");
        }
    }
}