using DiscomonProject.Discord;
using DiscomonProject.Discord.Entities;
using System;

namespace DiscomonProject
{
    internal class Program
    {
        private static void Main()
        {
            Unity.RegisterTypes();
            Console.WriteLine("Hello, Discord!");
            
            var discordBotConfig = new MonBotConfig
            {
                Token = "ABC",
                SocketConfig = SocketConfig.GetDefault()

            };
        }
    }

    
}
