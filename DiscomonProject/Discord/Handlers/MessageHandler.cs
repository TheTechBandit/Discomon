using System;
using System.Drawing;
using System.IO;
using System.Linq;
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

        public static async Task<string> GetImageURL(Bitmap img)
        {
            IUserMessage message = null;
            using(MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);
                img.Dispose();
                message = await _client.GetGuild(452818303635685386).GetTextChannel(735569092072964177).SendFileAsync(stream, "Image.png", "");
                stream.Close();
            }

            string url = "";
            foreach(IAttachment att in message.Attachments)
                url = att.Url;
            
            return url;
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

        public static async Task NotImplemented(ContextIds context, string msg)
        {
            await SendMessage(context, $"{msg.ToUpper()} not implemented yet!");
        }

        public static async Task AttackStepText(ContextIds context)
        {
            await MessageHandler.SendMessage(context, "Next turn starts! Choose attacks.");
        }

        public static async Task AttackEnteredText(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention}, your attack has been entered. Awaiting other player.");
        }

        public static async Task AttackEnteredTextNew(ContextIds context, UserAccount user, int num)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention}, your attack has been entered. Awaiting {num} other player(s).");
        }

        public static async Task AttackAlreadyEntered(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention}, you already entered an attack! Waiting on other player.");
        }

        public static async Task AttackInvalid(ContextIds context, UserAccount user)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention}, you cannot enter an attack right now!");
        }

        public static async Task Faint(ContextIds context, UserAccount user, BasicMon mon)
        {
            await MessageHandler.SendMessage(context, $"{user.Mention}'s {mon.Nickname} fainted!");
        }

        public static async Task OutOfMonsWinner(ContextIds context, UserAccount winner, UserAccount loser)
        {
            await MessageHandler.SendMessage(context, $"{loser.Mention} has run out of mon! {winner.Mention} wins!");
        }

        public static async Task FaintWinner(ContextIds context, UserAccount user, BasicMon mon)
        {
            await MessageHandler.SendMessage(context, $"{mon.Nickname} fainted! {user.Mention} wins!");
        }

        public static async Task UseMove(ContextIds context, BasicMon mon, BasicMon target, string move, string addon)
        {
            await MessageHandler.SendEmbedMessage(context, $"**{mon.Nickname}** used **{move}**!"+addon, MonEmbedBuilder.FieldMon(target));
        }

        public static async Task UseMoveNew(ContextIds context, BasicMon target, string addon)
        {
            await MessageHandler.SendEmbedMessage(context, addon, MonEmbedBuilder.FieldMon(target));
        }

        public static async Task TakesDamage(ContextIds context, BasicMon mon, string addon)
        {
            await MessageHandler.SendEmbedMessage(context, $"{mon.Nickname} takes damage!"+addon, MonEmbedBuilder.FieldMon(mon));
        }

        /*
        EMOJI ADDERS
        */

        public static async Task FightScreenEmojis(IUserMessage message)
        {
            await message.AddReactionAsync(new Emoji("⚔"));
            await message.AddReactionAsync(new Emoji("👜"));
            await message.AddReactionAsync(new Emoji("🔁"));
            await message.AddReactionAsync(new Emoji("🏃"));
        }

        public static async Task FightScreen(ulong userId)
        {
            var user = UserHandler.GetUser(userId);
            
            var message = await _client.GetUser(userId).SendMessageAsync("", false, MonEmbedBuilder.FightScreen(user.Char.ActiveMon));

            await FightScreenEmojis(message);

            user.ReactionMessages.Add(message.Id, 0);
        }

        public static async Task MoveScreen(ulong userId)
        {
            var user = UserHandler.GetUser(userId);
            
            var message = await _client.GetUser(userId).SendMessageAsync("", false, MonEmbedBuilder.MoveScreen(user.Char.ActiveMon));

            await message.AddReactionAsync(new Emoji("1\u20E3"));
            await message.AddReactionAsync(new Emoji("2\u20E3"));
            await message.AddReactionAsync(new Emoji("3\u20E3"));
            await message.AddReactionAsync(new Emoji("4\u20E3"));

            user.ReactionMessages.Add(message.Id, 1);
        }

        public static async Task FightScreenNew(ulong userId)
        {
            var user = UserHandler.GetUser(userId);
            
            var message = await _client.GetUser(userId).SendMessageAsync("", false, MonEmbedBuilder.FightScreen(user.Char.ActiveMons[0]));

            await message.AddReactionAsync(new Emoji("⚔"));
            await message.AddReactionAsync(new Emoji("👜"));
            await message.AddReactionAsync(new Emoji("🔁"));
            await message.AddReactionAsync(new Emoji("🏃"));

            user.ReactionMessages.Add(message.Id, 2);
        }

        public static async Task MoveScreenNew(ulong userId)
        {
            var user = UserHandler.GetUser(userId);

            var message = await _client.GetUser(userId).SendMessageAsync("", false, MonEmbedBuilder.MoveScreenNew(user.Char.ActiveMons[user.Char.MoveScreenNum]));

            await message.AddReactionAsync(new Emoji("1\u20E3"));
            await message.AddReactionAsync(new Emoji("2\u20E3"));
            await message.AddReactionAsync(new Emoji("3\u20E3"));
            await message.AddReactionAsync(new Emoji("4\u20E3"));
            await message.AddReactionAsync(new Emoji("⏮"));

            user.ReactionMessages.Add(message.Id, 3);
        }

        public static async Task TargetingScreen(ulong userId)
        {
            var user = UserHandler.GetUser(userId);
            
            var message = await _client.GetUser(userId).SendMessageAsync("", false, MonEmbedBuilder.TargetingScreen(user, user.Char.ActiveMons[user.Char.MoveScreenNum]));

            await message.AddReactionAsync(new Emoji("1\u20E3"));
            await message.AddReactionAsync(new Emoji("2\u20E3"));
            await message.AddReactionAsync(new Emoji("3\u20E3"));
            await message.AddReactionAsync(new Emoji("4\u20E3"));
            await message.AddReactionAsync(new Emoji("5\u20E3"));
            await message.AddReactionAsync(new Emoji("6\u20E3"));
            await message.AddReactionAsync(new Emoji("7\u20E3"));
            await message.AddReactionAsync(new Emoji("8\u20E3"));
            await message.AddReactionAsync(new Emoji("9\u20E3"));
            await message.AddReactionAsync(new Emoji("⏮"));
            await message.AddReactionAsync(new Emoji("⏭️"));

            user.ReactionMessages.Add(message.Id, 4);
        }

        public static async Task EmojiTest(ContextIds context)
        {
            var emoji = await GetEmoji(580944143287582740);
            await MessageHandler.SendMessage(context, "Test1 <:suki:580944143287582740>");
            await MessageHandler.SendMessage(context, "Test2 :suki:580944143287582740");
            await MessageHandler.SendMessage(context, $"Test3 {emoji.ToString()}");
            await MessageHandler.SendMessage(context, $"Test4 {emoji.Name}");
        }

        public static async Task ModifyAsyncTest(ContextIds context, ulong userId)
        {
            var user = UserHandler.GetUser(userId);

            var message = await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(
            "Modify Async Tester",
            embed: MonEmbedBuilder.ModifyAsyncTestPageOne())
            .ConfigureAwait(false);

            await message.AddReactionAsync(new Emoji("1\u20E3"));
            await message.AddReactionAsync(new Emoji("2\u20E3"));

            user.RemoveAllReactionMessages(1);

            user.ReactionMessages.Add(message.Id, 13);
        }

        public static async Task Menu(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);

            var message = await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(
            "",
            embed: MonEmbedBuilder.MainMenu())
            .ConfigureAwait(false);

            //Locations
            await message.AddReactionAsync(await GetEmoji(732673934184677557));
            //Party
            await message.AddReactionAsync(await GetEmoji(580944131535273991));
            //Bag
            await message.AddReactionAsync(await GetEmoji(732676561341251644));
            //Dex
            await message.AddReactionAsync(await GetEmoji(732679405704445956));
            //Team
            await message.AddReactionAsync(await GetEmoji(732682490833141810));
            //PvP
            await message.AddReactionAsync(await GetEmoji(732680927242878979));
            //Settings
            await message.AddReactionAsync(await GetEmoji(732683469485899826));

            user.RemoveAllReactionMessages(1);

            user.ReactionMessages.Add(message.Id, 1);
        }

        public static async Task PartyMenu(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);
            user.Char.SwapMode = false;
            user.Char.SwapMonNum = -1;

            string url = MessageHandler.GetImageURL(ImageGenerator.PartyMenu(user.Char.Party)).Result;
            var message = await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(
            "",
            embed: MonEmbedBuilder.PartyMenu(url))
            .ConfigureAwait(false);

            //Back Arrow
            await message.AddReactionAsync(await GetEmoji(735583967046271016));
            //Numbers
            for(int i = 1; i <= user.Char.Party.Count; i++)
            {
                await message.AddReactionAsync(new Emoji($"{i}\u20E3"));
            }
            //Swap
            await message.AddReactionAsync(await MessageHandler.GetEmoji(736070692373659730));

            user.RemoveAllReactionMessages(5);

            user.ReactionMessages.Add(message.Id, 5);
        }

        public static async Task TeamMenu(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);
            Team t = user.GetTeam();

            var message = await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(
            "",
            embed: MonEmbedBuilder.TeamMenu(user))
            .ConfigureAwait(false);

            user.RemoveAllReactionMessages(7);
            user.RemoveAllReactionMessages(8);
            user.RemoveAllReactionMessages(9);

            //Back
            await message.AddReactionAsync(await GetEmoji(735583967046271016));
            if(t != null)
            {
                if(t.CanInvite(user))
                {
                    //Invite
                    await message.AddReactionAsync(await GetEmoji(736476027886501888));
                }
                if(t.CanKick(user))
                {
                    //Kick
                    await message.AddReactionAsync(await GetEmoji(736476054427795509));
                }
                //Leave team
                await message.AddReactionAsync(await GetEmoji(736485364700545075));
                if(t.CanAccessSettings(user))
                {
                    //Settings
                    await message.AddReactionAsync(await GetEmoji(732683469485899826));
                }
                if(t.CanDisband(user))
                {
                    //Disband Team
                    await message.AddReactionAsync(await GetEmoji(736487511655841802));
                }
                user.ReactionMessages.Add(message.Id, 8);
            }
            else
            {
                //Create Team
                await message.AddReactionAsync(await GetEmoji(732682490833141810));
                user.ReactionMessages.Add(message.Id, 9);
            }
        }

    }
}