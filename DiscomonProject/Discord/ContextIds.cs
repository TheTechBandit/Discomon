using System;
using Discord.Commands;

namespace DiscomonProject.Discord
{
    public class ContextIds
    {
        public ulong UserId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong GuildId { get; set; }
        public ulong MessageId { get; set; }

        public ContextIds()
        {
            
        }

        public ContextIds(SocketCommandContext context)
        {
            UserId = context.User.Id;
            ChannelId = context.Channel.Id;
            GuildId = context.Guild.Id;
            MessageId = context.Message.Id;
        }
    }
}