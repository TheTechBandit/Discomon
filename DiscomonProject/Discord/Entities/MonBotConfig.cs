using Discord.WebSocket;

namespace DiscomonProject.Discord.Entities
{
    public class MonBotConfig
    {
        public string Token { get; set; }
        public DiscordSocketConfig SocketConfig { get; set; }
    }

}