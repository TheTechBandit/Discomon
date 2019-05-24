using DiscomonProject.Discord.Entities;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DiscomonProject.Discord
{
    public class Connection
    {
        private DiscordSocketClient _client;
        private DiscordLogger _logger;

        public Connection(DiscordLogger logger)
        {
            _logger = logger;
        }

        internal async Task ConnectAsync(MonBotConfig config)
        {
            _client = new DiscordSocketClient(config.SocketConfig);

            _client.Log += _logger.Log;

            // TODO: CONTINUE
        }

        private Task Log(LogMessage arg)
        {
            throw new NotImplementedException();
        }
    }
}