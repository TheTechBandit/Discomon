using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
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

        public static async Task MoveScreenEmojis(IUserMessage message)
        {
            await message.AddReactionAsync(new Emoji("1\u20E3"));
            await message.AddReactionAsync(new Emoji("2\u20E3"));
            await message.AddReactionAsync(new Emoji("3\u20E3"));
            await message.AddReactionAsync(new Emoji("4\u20E3"));
        }

        public static async Task MoveScreen(ulong userId)
        {
            var user = UserHandler.GetUser(userId);
            
            var message = await _client.GetUser(userId).SendMessageAsync("", false, MonEmbedBuilder.MoveScreen(user.Char.ActiveMon));

            await MoveScreenEmojis(message);

            user.ReactionMessages.Add(message.Id, 1);
        }

        public static async Task FightScreenNew(ulong userId)
        {
            var user = UserHandler.GetUser(userId);
            
            var message = await _client.GetUser(userId).SendMessageAsync("", false, MonEmbedBuilder.FightScreen(user.Char.ActiveMons[0]));

            await FightScreenEmojis(message);

            user.ReactionMessages.Add(message.Id, 2);
        }

        public static async Task MoveScreenNewEmojis(IUserMessage message)
        {
            await message.AddReactionAsync(new Emoji("1\u20E3"));
            await message.AddReactionAsync(new Emoji("2\u20E3"));
            await message.AddReactionAsync(new Emoji("3\u20E3"));
            await message.AddReactionAsync(new Emoji("4\u20E3"));
            await message.AddReactionAsync(new Emoji("⏮"));
        }

        public static async Task MoveScreenNew(ulong userId)
        {
            var user = UserHandler.GetUser(userId);

            var message = await _client.GetUser(userId).SendMessageAsync("", false, MonEmbedBuilder.MoveScreenNew(user.Char.ActiveMons[user.Char.MoveScreenNum]));

            await MoveScreenNewEmojis(message);

            user.ReactionMessages.Add(message.Id, 3);
        }

        public static async Task TargetingScreenEmojis(IUserMessage message)
        {
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
        }

        public static async Task TargetingScreen(ulong userId)
        {
            var user = UserHandler.GetUser(userId);
            
            var message = await _client.GetUser(userId).SendMessageAsync("", false, MonEmbedBuilder.TargetingScreen(user, user.Char.ActiveMons[user.Char.MoveScreenNum]));

            await TargetingScreenEmojis(message);

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

        public static async Task MenuEmojis(IUserMessage message)
        {
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
        }

        public static async Task Menu(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);

            var message = await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(
            "",
            embed: MonEmbedBuilder.MainMenu(user))
            .ConfigureAwait(false);

            await MenuEmojis(message);

            user.RemoveAllReactionMessages(1);

            user.ReactionMessages.Add(message.Id, 1);
        }

        public static async Task PartyMenuEmojis(IUserMessage message, UserAccount user)
        {
            //Back Arrow
            await message.AddReactionAsync(await GetEmoji(735583967046271016));
            //Numbers
            for(int i = 1; i <= user.Char.Party.Count; i++)
            {
                await message.AddReactionAsync(new Emoji($"{i}\u20E3"));
            }
            //Swap
            await message.AddReactionAsync(await MessageHandler.GetEmoji(736070692373659730));
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

            await PartyMenuEmojis(message, user);

            user.RemoveAllReactionMessages(5);

            user.ReactionMessages.Add(message.Id, 5);
        }

        public static async Task TeamMenuEmojis(IUserMessage message, UserAccount user)
        {
            Team t = user.GetTeam();
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
                user.ReactionMessages.Add(message.Id, 7);
            }
            else
            {
                //Create Team
                await message.AddReactionAsync(await GetEmoji(732682490833141810));
                //Join Open Team
                await message.AddReactionAsync(await GetEmoji(736476027886501888));
                user.ReactionMessages.Add(message.Id, 9);
            }
        }

        public static async Task TeamMenu(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);

            var message = await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(
            "",
            embed: MonEmbedBuilder.TeamMenu(user))
            .ConfigureAwait(false);

            await TeamMenuEmojis(message, user);

            user.RemoveAllReactionMessages(7);
            user.RemoveAllReactionMessages(8);
            user.RemoveAllReactionMessages(9);
        }

        public static async Task TeamInviteEmojis(IUserMessage message)
        {
            //Check
            await message.AddReactionAsync(await GetEmoji(736480922152730665));
            //X
            await message.AddReactionAsync(await GetEmoji(736481999006466119));
        }

        public static async Task TeamSettingsMenu(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);

            var message = await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(
            "",
            embed: MonEmbedBuilder.TeamSettingsMenu(user))
            .ConfigureAwait(false);

            await TeamSettingsEmojis(message, user);

            user.RemoveAllReactionMessages(7);
            user.RemoveAllReactionMessages(8);
            user.RemoveAllReactionMessages(9);

            user.ReactionMessages.Add(message.Id, 8);
        }

        public static async Task TeamSettingsEmojis(IUserMessage message, UserAccount user)
        {
            var team = user.GetTeam();
            if(team.CanAccessSettings(user))
            {
                //Back
                await message.AddReactionAsync(await GetEmoji(735583967046271016));
                //Edit
                await message.AddReactionAsync(await GetEmoji(736488507895447622));
                //AddImage
                await message.AddReactionAsync(await GetEmoji(736489932297863248));
                //RGB
                await message.AddReactionAsync(await GetEmoji(736489012595785818));
                //Permissions
                await message.AddReactionAsync(await GetEmoji(736476627675906078));
                //Lock
                if(team.OpenInvite)
                    await message.AddReactionAsync(await GetEmoji(736491703091069031));
                //Unlock
                else
                    await message.AddReactionAsync(await GetEmoji(736491688712732733));
            }
        }

        public static async Task PvPMainMenuEmojis(IUserMessage message)
        {
            //Back
            await message.AddReactionAsync(await GetEmoji(735583967046271016));
            //Single Battles
            await message.AddReactionAsync(await GetEmoji(752302449103995021));
            //Double Battles
            await message.AddReactionAsync(await GetEmoji(752302448768450560));
            //FFA
            await message.AddReactionAsync(await GetEmoji(752302449380819044));
            //Custom Battle
            await message.AddReactionAsync(await GetEmoji(752326873970770001));
        }

        public static async Task PvPMainMenu(ContextIds context)
        {
            var user = UserHandler.GetUser(context.UserId);

            var message = await _client.GetGuild(context.GuildId).GetTextChannel(context.ChannelId).SendMessageAsync(
            "",
            embed: MonEmbedBuilder.PvPMainMenu())
            .ConfigureAwait(false);

            await PvPMainMenuEmojis(message);

            user.RemoveAllReactionMessages(11);
        }

        public static async Task ParseExpectedInput(SocketUserMessage message, UserAccount user, SocketCommandContext Context)
        {
            ContextIds idList = new ContextIds(Context);
            if((user.ExpectedInputLocation == idList.ChannelId || user.ExpectedInputLocation == 0) && user.GetTeam() != null)
            {
                if(user.ExpectedInput == 0 || user.ExpectedInput == 1)
                {
                    //Update the menu(s) of the person who sent the invite
                    await UpdateMenu(user, message.Channel, 7, "");
                    /*foreach(KeyValuePair<ulong, int> kvp in user.ReactionMessages)
                    {
                        if(kvp.Value == 7)
                        {
                            IMessage teamMess = await message.Channel.GetMessageAsync(kvp.Key);
                            if(teamMess is IUserMessage)
                            {
                                IUserMessage userTeamMess = (IUserMessage)teamMess;
                                await userTeamMess.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamMenu(user); m.Content = "";});
                            }
                        }
                    }*/
                }
                //Team Invite Sending
                if(user.ExpectedInput == 0)
                {
                    foreach(SocketUser u in message.MentionedUsers)
                    {
                        var ua = UserHandler.GetUser(u.Id);
                        if(ua.GetTeam() == null && ua.Char != null)
                        {
                            var m = await _client.GetGuild(idList.GuildId).GetTextChannel(idList.ChannelId).SendMessageAsync("", embed: MonEmbedBuilder.TeamInviteMenu(user, ua)).ConfigureAwait(false);
                            await TeamInviteEmojis(m);
                            ua.ReactionMessages.Add(m.Id, 10);
                            ua.InviteMessages.Add(m.Id, user.UserId);
                        }
                    }
                }
                //Kick players from team
                else if(user.ExpectedInput == 1)
                {
                    //Kick all players who are on the team as long as the user kicking has kick permissions
                    foreach(SocketUser u in message.MentionedUsers)
                    {
                        var ua = UserHandler.GetUser(u.Id);
                        if(ua.GetTeam() == user.GetTeam() && user.GetTeam().CanKick(user) && user != ua)
                        {
                            ua.GetTeam().KickMember(ua);
                        }
                    }

                    //Update the user's currently active team menu
                    await UpdateMenu(user, message.Channel, 7, "");
                    /*foreach(KeyValuePair<ulong, int> kvp in user.ReactionMessages)
                    {
                        if(kvp.Value == 7)
                        {
                            IMessage teamMess = await message.Channel.GetMessageAsync(kvp.Key);
                            if(teamMess is IUserMessage)
                            {
                                IUserMessage userTeamMess = (IUserMessage)teamMess;
                                await userTeamMess.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamMenu(user); m.Content = "";});
                            }
                        }
                    }*/
                }
                //Edit team name
                else if(user.ExpectedInput == 2)
                {
                    var team = user.GetTeam();
                    if(team.CanAccessSettings(user) && message.Content.Length < 64)
                    {
                        team.TeamName = message.Content;
                    }
                    await UpdateMenu(user, message.Channel, 8, "");
                }
                //Add image url
                else if(user.ExpectedInput == 3)
                {
                    var team = user.GetTeam();
                    if(team.CanAccessSettings(user))
                    {
                        team.Picture = message.Content;
                    }
                    await UpdateMenu(user, message.Channel, 8, "");
                }
                //Edit RGB
                else if(user.ExpectedInput == 4)
                {
                    var team = user.GetTeam();
                    string updateContent = "";
                    if(team.CanAccessSettings(user))
                    {
                        string str = message.Content;
                        str = str.Replace(",", "");
                        string[] rgb = str.Split(' ');
                        if(rgb.Count() == 3)
                        {
                            try
                            {
                                team.TeamR = Int32.Parse(rgb[0]);
                                team.TeamG = Int32.Parse(rgb[1]);
                                team.TeamB = Int32.Parse(rgb[2]);
                            }
                            catch(FormatException e)
                            {
                                Console.WriteLine(e.Message);
                                updateContent = "**You didn't enter the RGB values correctly!**";
                            }
                        }
                    }
                    await UpdateMenu(user, message.Channel, 8, updateContent);
                }

                user.ExpectedInput = -1;
                user.ExpectedInputLocation = 0;
            }
            //Expected inputs that do not require a team
            if(user.ExpectedInputLocation == idList.ChannelId || user.ExpectedInputLocation == 0)
            {
                //Join open team
                if(user.ExpectedInput == 5)
                {
                    string updateContent = "";
                    var team = user.GetTeam();
                    UserAccount otherUser = null;
                    if(message.MentionedUsers.Count != 0)
                        otherUser = UserHandler.GetUser(message.MentionedUsers.First().Id);
                    Team otherTeam = null;
                    if(otherUser != null)
                        otherTeam = otherUser.GetTeam();

                    if(otherTeam != null)
                    {
                        if(otherTeam.OpenInvite)
                        {
                            if(team == null)
                            {
                                otherTeam.AddMember(user);
                            }
                            else
                                updateContent = "**You already have a team!**";
                        }
                        else
                                updateContent = "**That team is not open invite!**";
                    }
                    else
                                updateContent = "**That person does not have a team!**";

                    await UpdateMenu(user, message.Channel, 9, updateContent);
                }

                user.ExpectedInput = -1;
                user.ExpectedInputLocation = 0;
            }
        }

        public static async Task UpdateMenu(UserAccount user, ISocketMessageChannel channel, int num, string content)
        {
            //Update the user's currently active team menu
            foreach(KeyValuePair<ulong, int> kvp in user.ReactionMessages)
            {
                //Team menu
                if(kvp.Value == 7 && num == 7)
                {
                    IMessage teamMess = await channel.GetMessageAsync(kvp.Key);
                    if(teamMess is IUserMessage)
                    {
                        IUserMessage userTeamMess = (IUserMessage)teamMess;
                        await userTeamMess.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamMenu(user); m.Content = content;});
                    }
                }
                //Team settings
                if(kvp.Value == 8 && num == 8)
                {
                    IMessage teamMess = await channel.GetMessageAsync(kvp.Key);
                    if(teamMess is IUserMessage)
                    {
                        IUserMessage userTeamMess = (IUserMessage)teamMess;
                        await userTeamMess.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamSettingsMenu(user); m.Content = content;});
                    }
                }
                //Team creation (for updating it into becoming a Team Menu)
                if(kvp.Value == 9 && num == 9)
                {
                    IMessage teamMess = await channel.GetMessageAsync(kvp.Key);
                    if(teamMess is IUserMessage)
                    {
                        IUserMessage userTeamMess = (IUserMessage)teamMess;
                        await userTeamMess.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamMenu(user); m.Content = content;});
                        if(user.GetTeam() != null)
                        {
                            user.RemoveAllReactionMessages(7);
                            user.RemoveAllReactionMessages(9);
                            //FOR FUTURE USE- When bot has no permissions, this can be used to remove bot-only reactions
                            //await message.RemoveReactionAsync(await MessageHandler.GetEmoji(736480922152730665), message.Author);
                            await userTeamMess.RemoveAllReactionsAsync();
                            await MessageHandler.TeamMenuEmojis(userTeamMess, user);
                        }
                    }
                }
            }
        }

    }
}