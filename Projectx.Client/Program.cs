using Projectx.Entity.Models;

namespace Projectx.Client;

internal class Program
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            int inputCommand = DefineInputCommand();

            if (inputCommand == 4)
                break;

            MethodHelper.AppPath = Environment.GetEnvironmentVariable("PROJECTX_APP_PATH");
            
            Console.WriteLine($"app path: {MethodHelper.AppPath}");

            switch (inputCommand)
            {
                case 1:
                    await Send();
                    break;
                case 2:
                    await ViewHistory();
                    break;
            }
        }
    }

    private static int DefineInputCommand()
    {
        string commandListBanner =
            @"Please, select command:\n\t1) send - send your message\n\t2) view - view last 10 minutes history";

        Console.Clear();
        Console.WriteLine(commandListBanner);
        string inputLine = Console.ReadLine();

        if (int.TryParse(inputLine, out int result) && result >= 1 && result <= 3)
            return result;
        else
            return 4;
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
}
