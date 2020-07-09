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
                GuildId = 0
            };

            if(messageType == 0)
                await AttackScreen(user, message.Id, reaction.Emote, idList);
            //if(messageType == 1)
                //await MoveScreen(user, message.Id, reaction.Emote, idList);
            if(messageType == 2)
                await AttackScreenNew(user, message.Id, reaction.Emote, idList);
            if(messageType == 3)
                await MoveScreenNew(user, message.Id, reaction.Emote, idList);
            if(messageType == 4)
                await TargetingScreen(user, message.Id, reaction.Emote, idList);
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
                await MessageHandler.MoveScreenNew(user.UserId);
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

    }
}