using Projectx.Entity.Models;
using Projectx.Repository;

namespace RepositoryTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=123;";
            //string connectionString = "Host=127.0.0.1;Port=5432;Integrated Security=true;";
            var messageRepository = new MessageRepository();
            //await messageRepository.Seed(connectionString);
            var message = new Message
            {
                MessageId = Guid.NewGuid(),
                ClientId = 1,
                Created = DateTime.Now,
                Content = "Hello World"
            };
            await messageRepository.Create(message);

            var x = await messageRepository.GetAll();
            foreach (var y in x)
            {
                Console.WriteLine(y.MessageId);
                Console.WriteLine(y.ClientId);
                Console.WriteLine(y.Created);
                Console.WriteLine(y.Content);
            }

            Console.WriteLine("framing");

            var z = await messageRepository.GetByTimeframe(DateTime.Today.AddMinutes(-10), DateTime.Now);
            foreach (var y in z)
            {
                Console.WriteLine(y.MessageId);
                Console.WriteLine(y.ClientId);
                Console.WriteLine(y.Created);
                Console.WriteLine(y.Content);
            }
        }
    }
}
