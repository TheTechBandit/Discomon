using System.Threading.Tasks;
using Discord;
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

        public static async Task SendEmbedMessage(ContextIds context, string message, Embed emb)
        {
            await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(
            message,
            embed: emb)
            .ConfigureAwait(false);
        }

        public static async Task SendDM(ulong userId, string message)
        {
            await _client.GetUser(userId).SendMessageAsync(message);
        }

        public static async Task SendDM(ulong userId, string message, Embed emb)
        {
            await _client.GetUser(userId).SendMessageAsync(
                message,
                embed: emb)
                .ConfigureAwait(false);
        }


        /* PRESET MESSAGES */
        public static async Task CharacterMissing(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);
            await MessageHandler.SendMessage(context, $"{user.Mention}, you do not have a character! You can create one using the \"startadventure\" command.");
        }

        public static async Task OtherCharacterMissing(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);
            await MessageHandler.SendMessage(context, $"{user.Mention}, that user does not have a character!");
        }

        public static async Task InvalidCharacterLocation(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);
            await MessageHandler.SendMessage(context, $"{user.Mention} you must be in this location to use commands here! Your character is currently at {user.Char.CurrentGuildName}.");
        }

        public static async Task InvalidOtherCharacterLocation(ContextIds context, UserAccount otherUser)
        {
            var user = UserHandler.GetUser(context.UserId);
            await MessageHandler.SendMessage(context, $"{user.Mention} that player is not in this location! They are currently at {otherUser.Char.CurrentGuildName}.");
        }

        public static async Task AttackStepText(ContextIds context)
        {
            await MessageHandler.SendMessage(context, "Next turn starts! Choose attacks.");
        }

        public static async Task AttackEnteredText(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention}, your attack has been entered. Awaiting other player.");
        }

        public static async Task AttackAlreadyEntered(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention}, you already entered an attack! Waiting on other player.");
        }

        public static async Task AttackInvalid(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention}, you cannot enter an attack right now!");
        }

        public static async Task FaintWinner(ContextIds context, UserAccount user, BasicMon mon)
        {
            await MessageHandler.SendMessage(context, $"{mon.Nickname} fainted! {user.Mention} wins!");
        }

        public static async Task UseMove(ContextIds context, BasicMon mon, string move)
        {
            await MessageHandler.SendMessage(context, $"**{mon.Nickname}** used **{move}**!");
        }

        public static async Task TakesDamage(ContextIds context, BasicMon mon, string damage)
        {
            await MessageHandler.SendEmbedMessage(context, $"{mon.Nickname} takes {damage} damage!", MonEmbedBuilder.FieldMon(mon));
        }

        public static async Task FightScreen(ulong userId)
        {
            var user = UserHandler.GetUser(userId);

            var dm = await _client.GetUser(userId).GetOrCreateDMChannelAsync();
            var message = await dm.SendMessageAsync("", false, MonEmbedBuilder.FightScreen(user.Char.Party[0]));

            var fight = message.AddReactionAsync(new Emoji("⚔"));
            var bag = message.AddReactionAsync(new Emoji("👜"));
            var swap = message.AddReactionAsync(new Emoji("🔁"));
            var run = message.AddReactionAsync(new Emoji("🏃"));

            await Task.WhenAll(fight, bag, swap, run);

            user.ReactionMessages.Add(message.Id, 0);
        }

    }
}