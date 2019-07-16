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

        public static async Task SendMessage(ContextIds context, string message)
        {
            await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(message);
        }


        /* PRESET MESSAGES */
        public static async Task CharacterMissing(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention}, you do not have a character! You can create one using the \"startadventure\" command.");
        }

        public static async Task OtherCharacterMissing(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention}, that user does not have a character!");
        }

        public static async Task InvalidCharacterLocation(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention} you must be in this location to use commands here! Your character is currently at {user.Char.CurrentGuildName}.");
        }

        public static async Task InvalidOtherCharacterLocation(ContextIds context, UserAccount user, UserAccount otherUser)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention} that player is not in this location! They are currently at {otherUser.Char.CurrentGuildName}.");
        }

    }
}