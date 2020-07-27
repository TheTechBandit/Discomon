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
            //if(messageType == 8) TEAM SETTINGS
                //await TargetingScreen(user, message.Id, reaction.Emote, idList);
            if(messageType == 9)
                await CreateTeamMenu(user, message, reaction.Emote, idList);
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
                    //Back Arrow
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(735583967046271016));
                    //Numbers
                    for(int i = 1; i <= user.Char.Party.Count; i++)
                    {
                        await message.AddReactionAsync(new Emoji($"{i}\u20E3"));
                    }
                    //Swap
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(736070692373659730));
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
                    Team t = user.GetTeam();
                    //Back
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(735583967046271016));
                    if(t != null)
                    {
                        if(t.CanInvite(user))
                        {
                            //Invite
                            await message.AddReactionAsync(await MessageHandler.GetEmoji(736476027886501888));
                        }
                        if(t.CanKick(user))
                        {
                            //Kick
                            await message.AddReactionAsync(await MessageHandler.GetEmoji(736476054427795509));
                        }
                        //Leave team
                        await message.AddReactionAsync(await MessageHandler.GetEmoji(736485364700545075));
                        if(t.CanAccessSettings(user))
                        {
                            //Settings
                            await message.AddReactionAsync(await MessageHandler.GetEmoji(732683469485899826));
                        }
                        if(t.CanDisband(user))
                        {
                            //Disband Team
                            await message.AddReactionAsync(await MessageHandler.GetEmoji(736487511655841802));
                        }
                        user.ReactionMessages.Add(message.Id, 8);
                    }
                    else
                    {
                        //Create Team
                        await message.AddReactionAsync(await MessageHandler.GetEmoji(732682490833141810));
                        user.ReactionMessages.Add(message.Id, 9);
                    }
                    break;
                case "pvp":
                    await MessageHandler.NotImplemented(idList, "pvp");
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
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.MainMenu(); m.Content = "";});
                    //Locations
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732673934184677557));
                    //Party
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(580944131535273991));
                    //Bag
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732676561341251644));
                    //Dex
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732679405704445956));
                    //Team
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732682490833141810));
                    //PvP
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732680927242878979));
                    //Settings
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732683469485899826));
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
            switch(emote.Name.ToLower())
            {
                case "back1":
                    user.RemoveAllReactionMessages(7);
                    user.RemoveAllReactionMessages(1);
                    await message.RemoveAllReactionsAsync();
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.MainMenu(); m.Content = "";});
                    //Locations
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732673934184677557));
                    //Party
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(580944131535273991));
                    //Bag
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732676561341251644));
                    //Dex
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732679405704445956));
                    //Team
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732682490833141810));
                    //PvP
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732680927242878979));
                    //Settings
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732683469485899826));
                    user.ReactionMessages.Add(message.Id, 1);
                    break;
                case "invite":
                    await MessageHandler.NotImplemented(idList, "invite");
                    break;
                case "kick_player":
                    await MessageHandler.NotImplemented(idList, "kick_player");
                    break;
                case "exit":
                    await MessageHandler.NotImplemented(idList, "exit");
                    break;
                case "settings":
                    await MessageHandler.NotImplemented(idList, "settings");
                    break;
                case "disband":
                    await MessageHandler.NotImplemented(idList, "disband");
                    break;
                default:
                    break;
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
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.MainMenu(); m.Content = "";});
                    //Locations
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732673934184677557));
                    //Party
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(580944131535273991));
                    //Bag
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732676561341251644));
                    //Dex
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732679405704445956));
                    //Team
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732682490833141810));
                    //PvP
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732680927242878979));
                    //Settings
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732683469485899826));
                    user.ReactionMessages.Add(message.Id, 1);
                    break;
                case "team":
                    Team t = new Team(true);
                    t.AddMember(user);
                    TownHandler.GetTown(user.Char.CurrentGuildId).Teams.Add(t);

                    user.RemoveAllReactionMessages(9);
                    user.RemoveAllReactionMessages(7);
                    await message.RemoveAllReactionsAsync();
                    await message.ModifyAsync(m => {m.Embed = MonEmbedBuilder.TeamMenu(user); m.Content = "";});
                    //Back
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(735583967046271016));
                    //Invite
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(736476027886501888));
                    //Kick
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(736476054427795509));
                    //Leave team
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(736485364700545075));
                    //Settings
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(732683469485899826));
                    //Disband Team
                    await message.AddReactionAsync(await MessageHandler.GetEmoji(736487511655841802));
                    user.ReactionMessages.Add(message.Id, 7);
                    break;
                default:
                    break;
            }
        }

    }
}