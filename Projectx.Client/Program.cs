using Projectx.Entity.Models;

namespace Projectx.Client;

internal class Program
{
    static async Task Main(string[] args)
    {
        Task.Run(() => Watcher.Watch());

        while (true)
        {
            int inputCommand = DefineInputCommand();

            if (inputCommand == 4)
                break;

            switch (inputCommand)
            {
                case 1:
                    await Send();
                    break;
                case 2:
                    await ViewHistory();
                    break;
                case 3:
                    await WatchMessages();
                    break;
            }
        }
    }

    private static int DefineInputCommand()
    {
        string commandListBanner =
            @"Please, select command:
                1) send - send your message
                2) view - view last 10 minutes history
                3) watch - watch new incoming messages";

        Console.Clear();
        Console.WriteLine(commandListBanner);
        string inputLine = Console.ReadLine();

        if (int.TryParse(inputLine, out int result) && result >= 1 && result <= 3)
            return result;
        else
            return 0;
    }

    private async static Task Send()
    {
        Console.Clear();
        int clientId = await MethodHelper.Login();
        await MethodHelper.SendMessage(clientId);
        Console.ReadKey();
    }

    private async static Task ViewHistory()
    {
        Console.Clear();
        await MethodHelper.ViewHistory();
        Console.ReadKey();
    }

    private async static Task WatchMessages()
    {
        Console.Clear();
        while (true)
        {
            foreach(var message in Watcher.Messages)
                Console.WriteLine(message);
        }
        Console.ReadKey();
    }
}