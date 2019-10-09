using System;
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

        public static async Task<GuildEmote> GetEmoji(ulong id)
        {
            return await _client.GetGuild(452818303635685386).GetEmoteAsync(id);
        }
        
        public static async Task SendMessage(ulong guildID, ulong channelID, string message)
        {
            await _client.GetGuild(guildID).GetTextChannel(channelID).SendMessageAsync(message);
        }

        public static async Task SendMessage(ContextIds context, string message)
        {
            //If a GuildId is not provided, assume it is a DM channel.
            if(context.GuildId == 0)
            {
                await SendDM(context.UserId, message);
            }
            else
            {
                await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(message);
            }
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

        public static async Task NotInCombat(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);
            await MessageHandler.SendMessage(context, $"{user.Mention}, you are not in combat right now!");
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

        public static async Task TakesDamage(ContextIds context, BasicMon mon)
        {
            await MessageHandler.SendEmbedMessage(context, $"{mon.Nickname} takes damage!", MonEmbedBuilder.FieldMon(mon));
        }

        public static async Task FightScreen(ulong userId)
        {
            var user = UserHandler.GetUser(userId);
            
            var message = await _client.GetUser(userId).SendMessageAsync("", false, MonEmbedBuilder.FightScreen(user.Char.Party[0]));

            await message.AddReactionAsync(new Emoji("⚔"));
            await message.AddReactionAsync(new Emoji("👜"));
            await message.AddReactionAsync(new Emoji("🔁"));
            await message.AddReactionAsync(new Emoji("🏃"));

            user.ReactionMessages.Add(message.Id, 0);
        }

        public static async Task MoveScreen(ulong userId)
        {
            var user = UserHandler.GetUser(userId);
            
            var message = await _client.GetUser(userId).SendMessageAsync("", false, MonEmbedBuilder.MoveScreen(user.Char.Party[0]));

            await message.AddReactionAsync(new Emoji("1\u20E3"));
            await message.AddReactionAsync(new Emoji("2\u20E3"));
            await message.AddReactionAsync(new Emoji("3\u20E3"));
            await message.AddReactionAsync(new Emoji("4\u20E3"));

            user.ReactionMessages.Add(message.Id, 1);
        }

        public static async Task EmojiTest(ContextIds context)
        {
            var emoji = await GetEmoji(580944143287582740);
            await MessageHandler.SendMessage(context, "Test1 <:suki:580944143287582740>");
            await MessageHandler.SendMessage(context, "Test2 :suki:580944143287582740");
            await MessageHandler.SendMessage(context, $"Test3 {emoji.ToString()}");
        }

    }
}