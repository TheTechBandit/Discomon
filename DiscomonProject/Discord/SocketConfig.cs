using Discord;
using Discord.WebSocket;

namespace DiscomonProject.Discord
{
    public static class SocketConfig
    {
        public static DiscordSocketConfig GetDefault() =>
            new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            };

        public static DiscordSocketConfig GetNew() => new DiscordSocketConfig();
    }
}