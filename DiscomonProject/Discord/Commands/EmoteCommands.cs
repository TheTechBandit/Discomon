using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using DiscomonProject.Storage.Implementations;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscomonProject.Discord
{
    public static class EmoteCommands
    {
        static EmoteCommands()
        {

        }

        public static async Task ParseEmote(UserAccount user, IUserMessage message, SocketReaction reaction)
        {
            var messageType = user.ReactionMessages[message.Id];
            ContextIds idList = new ContextIds()
            {
                UserId = reaction.UserId,
                ChannelId = reaction.Channel.Id,
                GuildId = user.Char.CurrentGuildId
            };

            if(messageType == 0)
                await AttackScreen(user, message.Id, reaction.Emote, idList);
            if(messageType == 1)
                await MainMenu(user, message, reaction.Emote, idList);
            if(messageType == 2)
                await AttackScreenNew(user, message.Id, reaction.Emote, idList);
            if(messageType == 3)
                await MoveScreenNew(user, message.Id, reaction.Emote, idList);
            if(messageType == 4)
                await TargetingScreen(user, message.Id, reaction.Emote, idList);
            if(messageType == 5)
                await PartyMenu(user, message, reaction.Emote, idList);
            //if(messageType == 6) MONSTAT
                //await TargetingScreen(user, message.Id, reaction.Emote, idList);
            if(messageType == 7)
                await TeamMenu(user, message, reaction.Emote, idList);
            if(messageType == 8)
                await TeamSettingsMenu(user, message, reaction.Emote, idList);
            if(messageType == 9)
                await CreateTeamMenu(user, message, reaction.Emote, idList);
            if(messageType == 10)
                await TeamInviteMenu(user, message, reaction.Emote, idList);
            if(messageType == 11)
                await PvPMainMenu(user, message, reaction.Emote, idList);
            if(messageType == 13)
                await ModifyAsyncTest(user, message, reaction.Emote, idList);
        }

        public static async Task AttackScreen(UserAccount user, ulong messageId, IEmote emote, ContextIds idList)
        {
            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.CharacterExists(idList);
                await UserHandler.CharacterInCombat(idList);
            }
            catch(InvalidCharacterStateException)
            {
                return;
            }
            
            if(emote.Name == "⚔")
            {
                await MessageHandler.MoveScreen(user.UserId);
                user.ReactionMessages.Remove(messageId);
            }
            else if(emote.Name == "👜")
            {
                await MessageHandler.SendDM(user.UserId, "BAG not implemented yet!");
            }
            else if(emote.Name == "🔁")
            {
                await MessageHandler.SendDM(user.UserId, "SWITCH not implemented yet!");
            }
            else if(emote.Name == "🏃")
            {
                await MessageHandler.SendDM(user.UserId, "RUN not implemented yet!");
            }
        }

        public static async Task AttackScreenNew(UserAccount user, ulong messageId, IEmote emote, ContextIds idList)
        {
            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.CharacterExists(idList);
                await UserHandler.CharacterInCombat(idList);
            }
            catch(InvalidCharacterStateException)
            {
                return;
            }
            
            if(emote.Name == "⚔")
            {
                if(user.Char.ActiveMons[user.Char.MoveScreenNum].BufferedMove != null)
                {
                    user.Char.MoveScreenNum++;
                    if(user.Char.MoveScreenNum > CombatHandler2.GetInstance(user.Char.CombatId).GetTeam(user).MultiNum-1)
                    {
                        user.Char.MoveScreenNum = 0;
                    }
                }
                else
                {
                    await MessageHandler.MoveScreenNew(user.UserId);
                }
                user.ReactionMessages.Remove(messageId);
            }
            else if(emote.Name == "👜")
            {
                await MessageHandler.SendDM(user.UserId, "BAG not implemented yet!");
            }
            else if(emote.Name == "🔁")
            {
                await MessageHandler.SendDM(user.UserId, "SWITCH not implemented yet!");
            }
            else if(emote.Name == "🏃")
            {
                await MessageHandler.SendDM(user.UserId, "RUN not implemented yet!");
            }
        }

        public static async Task MoveScreenNew(UserAccount user, ulong messageId, IEmote emote, ContextIds idList)
        {
            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.CharacterExists(idList);
                await UserHandler.CharacterInCombat(idList);
            }
            catch(InvalidCharacterStateException)
            {
                return;
            }
            
            var num = user.Char.MoveScreenNum;
            
            if(emote.Name == "1\u20E3")
            {
                if(user.Char.ActiveMons[num].ActiveMoves[0].Name != "None")
                {
                    user.ReactionMessages.Remove(messageId);
                    await CombatHandler2.ParseMoveSelection(user, 0);
                }
            }
            else if(emote.Name == "2\u20E3")
            {
                if(user.Char.ActiveMons[num].ActiveMoves[1].Name != "None")
                {
                    user.ReactionMessages.Remove(messageId);
                    await CombatHandler2.ParseMoveSelection(user, 1);
                }
            }
            else if(emote.Name == "3\u20E3")
            {
                if(user.Char.ActiveMons[num].ActiveMoves[2].Name != "None")
                {
                    user.ReactionMessages.Remove(messageId);
                    await CombatHandler2.ParseMoveSelection(user, 2);
                }
            }
            else if(emote.Name == "4\u20E3")
            {
                if(user.Char.ActiveMons[num].ActiveMoves[3].Name != "None")
                {
                    user.ReactionMessages.Remove(messageId);
                    await CombatHandler2.ParseMoveSelection(user, 3);
                }
            }
            else if(emote.Name == "⏮")
            {
                if(user.Char.MoveScreenNum > 0)
                {
                    user.ReactionMessages.Remove(messageId);
                    user.Char.MoveScreenNum--;
                    await MessageHandler.MoveScreenNew(user.UserId);
                }
                else
                {
                    user.ReactionMessages.Remove(messageId);
                    await MessageHandler.FightScreenNew(user.UserId);
                }
            }
        }

        public static async Task TargetingScreen(UserAccount user, ulong messageId, IEmote emote, ContextIds idList)
        {
            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.CharacterExists(idList);
                await UserHandler.CharacterInCombat(idList);
            }
            catch(InvalidCharacterStateException)
            {
                return;
            }

            var mon = user.Char.ActiveMons[user.Char.MoveScreenNum];
            var inst = CombatHandler2.GetInstance(user.Char.CombatId);

            switch(emote.Name)
            {
                case "1\u20E3":
                    user.ReactionMessages.Remove(messageId);
                    mon.SelectedMove.Targets.Add(mon.SelectedMove.ValidTargets[0+user.Char.TargetPage*9]);
                    break;
                case "2\u20E3":
                    user.ReactionMessages.Remove(messageId);
                    mon.SelectedMove.Targets.Add(mon.SelectedMove.ValidTargets[1+user.Char.TargetPage*9]);
                    break;
                case "3\u20E3":
                    user.ReactionMessages.Remove(messageId);
                    mon.SelectedMove.Targets.Add(mon.SelectedMove.ValidTargets[2+user.Char.TargetPage*9]);
                    break;
                case "4\u20E3":
                    user.ReactionMessages.Remove(messageId);
                    mon.SelectedMove.Targets.Add(mon.SelectedMove.ValidTargets[3+user.Char.TargetPage*9]);
                    break;
                case "5\u20E3":
                    user.ReactionMessages.Remove(messageId);
                    mon.SelectedMove.Targets.Add(mon.SelectedMove.ValidTargets[4+user.Char.TargetPage*9]);
                    break;
                case "6\u20E3":
                    user.ReactionMessages.Remove(messageId);
                    mon.SelectedMove.Targets.Add(mon.SelectedMove.ValidTargets[5+user.Char.TargetPage*9]);
                    break;
                case "7\u20E3":
                    user.ReactionMessages.Remove(messageId);
                    mon.SelectedMove.Targets.Add(mon.SelectedMove.ValidTargets[6+user.Char.TargetPage*9]);
                    break;
                case "8\u20E3":
                    user.ReactionMessages.Remove(messageId);
                    mon.SelectedMove.Targets.Add(mon.SelectedMove.ValidTargets[7+user.Char.TargetPage*9]);
                    break;
                case "9\u20E3":
                    user.ReactionMessages.Remove(messageId);
                    mon.SelectedMove.Targets.Add(mon.SelectedMove.ValidTargets[8+user.Char.TargetPage*9]);
                    break;
                case "⏮":
                    user.ReactionMessages.Remove(messageId);
                    await MessageHandler.MoveScreenNew(user.UserId);
                    return;
                case "⏭️":
                    user.ReactionMessages.Remove(messageId);
                    user.Char.TargetPage++;
                    if(user.Char.TargetPage > (Math.Ceiling(mon.SelectedMove.ValidTargets.Count/9.0)))
                    {
                        user.Char.TargetPage =  0;
                    }
                    await MessageHandler.TargetingScreen(user.UserId);
                    return;
            }

            user.Char.MoveScreenNum++;
            if(user.Char.MoveScreenNum > inst.GetTeam(user).MultiNum--)
            {
                user.Char.MoveScreenNum = 0;
                await inst.ResolvePhase();
            }
            else
            {
                await MessageHandler.MoveScreenNew(user.UserId);
            }

        }

        public static async Task ModifyAsyncTest(UserAccount user, IUserMessage message, IEmote emote, ContextIds idList)
        {
            switch(emote.Name)
            {
                case "1\u20E3":
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.ModifyAsyncTestPageOne();});
                    break;
                case "2\u20E3":
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.ModifyAsyncTestPageTwo();});
                    break;
            }
        }

        public static async Task MainMenu(UserAccount user, IUserMessage message, IEmote emote, ContextIds idList)
        {
            switch(emote.Name.ToLower())
            {
                case "location":
                    await MessageHandler.NotImplemented(idList, "location");
                    break;
                case "snoril":
                    user.RemoveAllReactionMessages(1);
                    user.RemoveAllReactionMessages(5);

                    await message.RemoveAllReactionsAsync();
                    string url = MessageHandler.GetImageURL(ImageGenerator.PartyMenu(user.Char.Party)).Result;
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.PartyMenu(url); m.Content = "";});
                    await MessageHandler.PartyMenuEmojis(message, user);

                    user.ReactionMessages.Add(message.Id, 5);
                    break;
                case "bag":
                    await MessageHandler.NotImplemented(idList, "bag");
                    break;
                case "dex":
                    await MessageHandler.NotImplemented(idList, "dex");
                    break;
                case "team":
                    user.RemoveAllReactionMessages(7);
                    user.RemoveAllReactionMessages(8);
                    user.RemoveAllReactionMessages(9);
                    user.RemoveAllReactionMessages(1);

                    await message.RemoveAllReactionsAsync();
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamMenu(user); m.Content = "";});
                    await MessageHandler.TeamMenuEmojis(message, user);
                    //Reactionmessage added within TeamMenuEmojis() method
                    break;
                case "pvp":
                    user.RemoveAllReactionMessages(11);
                    user.RemoveAllReactionMessages(1);

                    await message.RemoveAllReactionsAsync();
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.PvPMainMenu(); m.Content = "";});
                    await MessageHandler.PvPMainMenuEmojis(message);
                    user.ReactionMessages.Add(message.Id, 11);
                    break;
                case "settings":
                    await MessageHandler.NotImplemented(idList, "settings");
                    break;
                default:
                    break;
            }
        }

        public static async Task PartyMenu(UserAccount user, IUserMessage message, IEmote emote, ContextIds idList)
        {
            switch(emote.Name.ToLower())
            {
                case "back1":
                    user.RemoveAllReactionMessages(5);
                    user.RemoveAllReactionMessages(1);

                    await message.RemoveAllReactionsAsync();
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.MainMenu(user); m.Content = "";});
                    await MessageHandler.MenuEmojis(message);

                    user.ReactionMessages.Add(message.Id, 1);
                    user.Char.SwapMode = false;
                    user.Char.SwapMonNum = -1;
                    break;
                case "1\u20E3":
                case "2\u20E3":
                case "3\u20E3":
                case "4\u20E3":
                case "5\u20E3":
                case "6\u20E3":
                    int num = int.Parse(emote.Name.ToLower().Substring(0, 1));
                    if(user.Char.Party.Count >= num)
                    {
                        if(!user.Char.SwapMode)
                            await MessageHandler.NotImplemented(idList, "monstats");
                        else
                        {
                            if(user.Char.SwapMonNum == -1)
                            {
                                user.Char.SwapMonNum = num-1;
                                await message.ModifyAsync(m => {m.Content = $"**Who should {user.Char.Party[num-1].Nickname} be swapped with?**";});
                            }
                            else
                            {
                                if(num-1 != user.Char.SwapMonNum)
                                {
                                    BasicMon temp = user.Char.Party[num-1];
                                    user.Char.Party[num-1] = user.Char.Party[user.Char.SwapMonNum];
                                    user.Char.Party[user.Char.SwapMonNum] = temp;
                                    string url = MessageHandler.GetImageURL(ImageGenerator.PartyMenu(user.Char.Party)).Result;
                                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.PartyMenu(url); m.Content = "";});
                                    user.Char.SwapMode = false;
                                    user.Char.SwapMonNum = -1;
                                }
                            }
                        }
                    }
                    
                    break;
                case "swap":
                    if(!user.Char.SwapMode)
                    {
                        await message.ModifyAsync(m => {m.Content = "**Swapping Mode Enabled**";});
                        user.Char.SwapMode = true;
                        user.Char.SwapMonNum = -1;
                    }
                    else
                    {
                        //Careful- m.Content string has an invisible EMPTY CHARACTER in it. Looks like this -->‎
                        await message.ModifyAsync(m => {m.Content = "‎";});
                        user.Char.SwapMode = false;
                        user.Char.SwapMonNum = -1;
                    }
                    break;
                default:
                    break;
            }
        }

        public static async Task TeamMenu(UserAccount user, IUserMessage message, IEmote emote, ContextIds idList)
        {
            Team team = user.GetTeam();
            if(team != null)
            {
                switch(emote.Name.ToLower())
                {
                    case "back1":
                        user.RemoveAllReactionMessages(7);
                        user.RemoveAllReactionMessages(1);

                        await message.RemoveAllReactionsAsync();
                        await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.MainMenu(user); m.Content = "";});
                        await MessageHandler.MenuEmojis(message);

                        user.ReactionMessages.Add(message.Id, 1);
                        break;
                    case "invite":
                        if(team.CanInvite(user))
                        {
                            await message.ModifyAsync(m => {m.Content = "**Please tag the player(s) you wish to invite.**";});
                            user.ExpectedInput = 0;
                            user.ExpectedInputLocation = message.Channel.Id;
                        }
                        break;
                    case "kick_player":
                        if(team.CanKick(user))
                        {
                            await message.ModifyAsync(m => {m.Content = "**Please tag the player(s) you wish to kick.**";});
                            user.ExpectedInput = 1;
                            user.ExpectedInputLocation = message.Channel.Id;
                        }
                        break;
                    case "exit":
                        bool leader = false;
                        if(team.Members.IndexOf(user) == 0)
                            leader = true;
                            
                        team.KickMember(user);

                        if(team.Members.Count > 0)
                        {
                            if(leader)
                                await MessageHandler.SendMessage(user.Char.CurrentGuildId, message.Channel.Id, $"{team.Members[0].Mention}, you are now the team leader.");
                        }
                        else
                        {
                            TownHandler.GetTown(user.Char.CurrentGuildId).Teams.Remove(team);
                        }

                        user.RemoveAllReactionMessages(9);
                        user.RemoveAllReactionMessages(7);

                        await message.RemoveAllReactionsAsync();
                        await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamMenu(user); m.Content = "";});
                        await MessageHandler.TeamMenuEmojis(message, user);

                        break;
                    case "settings":
                        if(team.CanAccessSettings(user))
                        {
                            user.RemoveAllReactionMessages(7);
                            user.RemoveAllReactionMessages(8);

                            await message.RemoveAllReactionsAsync();
                            await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamSettingsMenu(user); m.Content = "";});
                            await MessageHandler.TeamSettingsEmojis(message, user);

                            user.ReactionMessages.Add(message.Id, 8);
                        }
                        break;
                    case "disband":
                        if(team.CanDisband(user))
                        {
                            TownHandler.GetTown(user.Char.CurrentGuildId).Teams.Remove(team);

                            user.RemoveAllReactionMessages(9);
                            user.RemoveAllReactionMessages(7);

                            await message.RemoveAllReactionsAsync();
                            await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamMenu(user); m.Content = "";});
                            await MessageHandler.TeamMenuEmojis(message, user);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public static async Task TeamSettingsMenu(UserAccount user, IUserMessage message, IEmote emote, ContextIds idList)
        {
            Team team = user.GetTeam();
            if(team != null)
            {
                switch(emote.Name.ToLower())
                {
                    case "back1":
                        user.RemoveAllReactionMessages(8);
                        user.RemoveAllReactionMessages(7);

                        await message.RemoveAllReactionsAsync();
                        await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamMenu(user); m.Content = "";});
                        await MessageHandler.TeamMenuEmojis(message, user);
                        break;
                    case "edit":
                        if(team.CanAccessSettings(user))
                        {
                            await message.ModifyAsync(m => {m.Content = "**Type your team's name. It must be less than 64 charcters.**";});
                            user.ExpectedInput = 2;
                            user.ExpectedInputLocation = message.Channel.Id;
                        }
                        break;
                    case "addpicturegreen":
                        if(team.CanAccessSettings(user))
                        {
                            await message.ModifyAsync(m => {m.Content = "**Enter your team's picture as an image URL**";});
                            user.ExpectedInput = 3;
                            user.ExpectedInputLocation = message.Channel.Id;
                        }
                        break;
                    case "rgb":
                        if(team.CanAccessSettings(user))
                        {
                            await message.ModifyAsync(m => {m.Content = "**Enter RGB values separated with spaces. Ex: 91 255 10**";});
                            user.ExpectedInput = 4;
                            user.ExpectedInputLocation = message.Channel.Id;
                        }
                        break;
                    case "permissions":
                        if(team.CanAccessSettings(user))
                        {
                            if(team.Permissions == "OwnerOnly")
                            {
                                team.Permissions = "AllMembers";
                            }
                            else if(team.Permissions == "AllMembers")
                            {
                                team.Permissions = "NoPerms";
                            }
                            else if(team.Permissions == "NoPerms")
                            {
                                team.Permissions = "OwnerOnly";
                            }
                            await MessageHandler.UpdateMenu(user, (ISocketMessageChannel)message.Channel, 8, "");
                        }
                        break;
                    case "locked":
                    case "unlocked":
                        if(team.CanAccessSettings(user))
                        {
                            if(team.OpenInvite)
                                team.OpenInvite = false;
                            else
                                team.OpenInvite = true;
                            await MessageHandler.UpdateMenu(user, (ISocketMessageChannel)message.Channel, 8, "");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public static async Task CreateTeamMenu(UserAccount user, IUserMessage message, IEmote emote, ContextIds idList)
        {
            switch(emote.Name.ToLower())
            {
                case "back1":
                    user.RemoveAllReactionMessages(9);
                    user.RemoveAllReactionMessages(1);

                    await message.RemoveAllReactionsAsync();
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.MainMenu(user); m.Content = "";});
                    await MessageHandler.MenuEmojis(message);

                    user.ReactionMessages.Add(message.Id, 1);
                    break;
                case "team":
                    Team t = new Team(true);
                    t.AddMember(user);
                    TownHandler.GetTown(user.Char.CurrentGuildId).Teams.Add(t);

                    if(user.ExpectedInput == 5)
                    {
                        user.ExpectedInput = -1;
                        user.ExpectedInputLocation = 0;
                    }

                    user.RemoveAllReactionMessages(9);
                    user.RemoveAllReactionMessages(7);

                    await message.RemoveAllReactionsAsync();
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamMenu(user); m.Content = "";});
                    await MessageHandler.TeamMenuEmojis(message, user);
                    break;
                case "invite":
                    await message.ModifyAsync(m => {m.Content = "**Tag someone in the team you want to join. The team must be open.**";});
                    user.ExpectedInput = 5;
                    user.ExpectedInputLocation = message.Channel.Id;
                    break;
                default:
                    break;
            }
        }

        public static async Task TeamInviteMenu(UserAccount user, IUserMessage message, IEmote emote, ContextIds idList)
        {
            switch(emote.Name.ToLower())
            {
                case "check":
                    if(user.GetTeam() == null)
                    {
                        if(user.InviteMessages.ContainsKey(message.Id))
                        {
                            var otherUser = UserHandler.GetUser(user.InviteMessages[message.Id]);
                            if(otherUser.GetTeam() != null)
                            {
                                user.InviteMessages.Remove(message.Id);
                                user.RemoveAllReactionMessages(10);
                                otherUser.GetTeam().AddMember(user);
                                await message.RemoveAllReactionsAsync();
                                //FOR FUTURE USE- When bot has no permissions, this can be used to remove bot-only reactions
                                //await message.RemoveReactionAsync(await MessageHandler.GetEmoji(736480922152730665), message.Author);
                                await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamMenu(user); m.Content = "";});
                                await MessageHandler.TeamMenuEmojis(message, user);

                                //Update the menu of the person who sent the invite
                                foreach(KeyValuePair<ulong, int> kvp in otherUser.ReactionMessages)
                                {
                                    if(kvp.Value == 7)
                                    {
                                        IMessage teamMess = await message.Channel.GetMessageAsync(kvp.Key);
                                        if(teamMess is IUserMessage)
                                        {
                                            IUserMessage userTeamMess = (IUserMessage)teamMess;
                                            await userTeamMess.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamMenu(otherUser); m.Content = "";});
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "redx":
                    user.InviteMessages.Remove(message.Id);
                    user.RemoveAllReactionMessages(10);
                    await message.ModifyAsync(m => {m.Content = "Invite Declined.";});
                    break;
                default:
                    break;
            }
        }

        public static async Task PvPMainMenu(UserAccount user, IUserMessage message, IEmote emote, ContextIds idList)
        {
            switch(emote.Name.ToLower())
            {
                case "back1":
                    user.RemoveAllReactionMessages(11);
                    user.RemoveAllReactionMessages(1);

                    await message.RemoveAllReactionsAsync();
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.MainMenu(user); m.Content = "";});
                    await MessageHandler.MenuEmojis(message);

                    user.ReactionMessages.Add(message.Id, 1);
                    break;
                case "singlebattle":
                    await MessageHandler.NotImplemented(idList, "single battle");
                    break;
                case "doublebattle":
                    await MessageHandler.NotImplemented(idList, "double battle");
                    break;
                case "ffa":
                    await MessageHandler.NotImplemented(idList, "free for all");
                    break;
                case "custombattle":
                    await MessageHandler.NotImplemented(idList, "custom battle");
                    break;
                default:
                    break;
            }
        }

    }
}