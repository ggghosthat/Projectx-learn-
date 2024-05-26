using Projectx.Entity.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Projectx.Client
{
    internal class MethodHelper
    {
        public static string AppPath { get; set; }

        public async static ValueTask<int> Login()
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
                var response = await client.PostAsJsonAsync(AppPath + "/api/Gate/RegisterUser", registerModel);
                string json = await response.Content.ReadAsStringAsync();
                clientId = JsonSerializer.Deserialize<int>(json);
            }

            return clientId;
        }

        public async static Task SendMessage(int clientId)
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
                var response = await client.PostAsJsonAsync(AppPath + "/api/Message/SendMessage", messageModel);

                if (response.StatusCode == HttpStatusCode.OK)
                    Console.WriteLine("Message sent successfully.");
                else
                    Console.WriteLine("Something went wrong.");
            }
        }

        public async static Task ViewHistory()
        {
            Console.WriteLine("Last 10 minutes history:\n");
            using (var client = new HttpClient())
            {
                var responseMessages = await client.GetFromJsonAsync<IEnumerable<Message>>(AppPath + "/api/Message/LastHistory");

                foreach (var message in responseMessages)
                    Console.WriteLine($"MessageId: {message.MessageId}\n ClientId: {message.ClientId}\n Created: {message.Created}\n Content: {message.Content}\n");
            }
        }
    }
}
