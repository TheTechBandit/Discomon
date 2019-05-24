using DiscomonProject.Discord;
using DiscomonProject.Discord.Entities;
using DiscomonProject.Storage;
using System;
using System.Threading.Tasks;

namespace DiscomonProject
{
    internal class Program
    {
        private static async Task Main()
        {
            Unity.RegisterTypes();
            Console.WriteLine("Hello, Discord!");

            var storage = Unity.Resolve<IDataStorage>();
            
            var connection = Unity.Resolve<Connection>();
            await connection.ConnectAsync(new MonBotConfig
            {
                Token = storage.RestoreObject<string>("Config/BotToken")
            });

            Console.ReadKey();
        }
    }

    
}
