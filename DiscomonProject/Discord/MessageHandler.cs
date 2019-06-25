using System.Threading.Tasks;
using Discord.WebSocket;

namespace DiscomonProject.Discord
{
    public static class MessageHandler
    {
        private static DiscordSocketClient _client;

        static MessageHandler()
        {
            _client = Unity.Resolve<DiscordSocketClient>();
        }
        
        public static async Task SendMessage(ulong guildID, ulong channelID, string message)
        {
            await _client.GetGuild(guildID).GetTextChannel(channelID).SendMessageAsync(message);
        }


    }
}