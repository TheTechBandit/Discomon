using Discord;
using System.Threading.Tasks;

namespace DiscomonProject.Discord
{
    public class DiscordLogger
    {
        private readonly ILogger _logger;

        public DiscordLogger(ILogger logger) 
            => _logger = logger;

        public Task Log(LogMessage logMsg)
        {
            _logger.Log(logMsg.Message);
            return Task.CompletedTask;
        }
    }
}