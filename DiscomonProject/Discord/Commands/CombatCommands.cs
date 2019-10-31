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
    public class CombatCommands : ModuleBase<SocketCommandContext>
    {
        [Command("duel")]
        public async Task Duel(SocketGuildUser target)
        {
            var fromUser = UserHandler.GetUser(Context.User.Id);
            var toUser = UserHandler.GetUser(target.Id);

            ContextIds idList = new ContextIds(Context);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.CharacterExists(idList);
                await UserHandler.OtherCharacterExists(idList, toUser);
                await UserHandler.ValidCharacterLocation(idList);
                await UserHandler.OtherCharacterLocation(idList, toUser);
            }
            catch(InvalidCharacterStateException)
            {
                return;
            }

            //Check that the user did not target themself with the command
            if(fromUser.UserId != toUser.UserId)
            {
                //Set the current user's combat request ID to the user specified
                fromUser.Char.CombatRequest = toUser.UserId;

                //Check if the specified user has a combat request ID that is the current user's ID
                if(toUser.Char.CombatRequest == fromUser.UserId)
                {
                    //Make sure neither users are in combat while sending response request
                    if(fromUser.Char.InCombat)
                    {
                        await Context.Channel.SendMessageAsync($"{Context.User.Mention}, you cannot start a duel while in combat!");
                    }
                    else if(toUser.Char.InCombat)
                    {
                        await Context.Channel.SendMessageAsync($"{Context.User.Mention}, you cannot start a duel with a player who is in combat!");
                    }
                    else
                    {
                        //Start duel
                        /*
                        CombatInstance combat = new CombatInstance(idList, Context.User.Id, target.Id);

                        await Context.Channel.SendMessageAsync($"The duel between {target.Mention} and {Context.User.Mention} will now begin!");
                        fromUser.Char.InCombat = true;
                        fromUser.Char.InPvpCombat = true;
                        fromUser.Char.CombatRequest = 0;
                        fromUser.Char.InCombatWith = toUser.UserId;
                        fromUser.Char.Combat = new CombatInstance(idList, fromUser.UserId, toUser.UserId);

                        toUser.Char.InCombat = true;
                        toUser.Char.InPvpCombat = true;
                        toUser.Char.CombatRequest = 0;
                        toUser.Char.InCombatWith = fromUser.UserId;
                        toUser.Char.Combat = new CombatInstance(idList, toUser.UserId, fromUser.UserId);

                        await CombatHandler.StartCombat(fromUser.Char.Combat);
                        */
                        CombatInstance combat = new CombatInstance(idList, fromUser, toUser);

                        CombatHandler.StoreInstance(CombatHandler.NumberOfInstances(), combat);
                        await CombatHandler.StartCombat(combat);
                    }
                }
                else
                {
                    //Make sure neither users are in combat while sending initial request
                    if(fromUser.Char.InCombat)
                    {
                        await Context.Channel.SendMessageAsync($"{Context.User.Mention}, you cannot request a duel when you are in combat!");
                    }
                    else if(toUser.Char.InCombat)
                    {
                        await Context.Channel.SendMessageAsync($"{Context.User.Mention}, you cannot duel a player who is in combat!");
                    }
                    else
                    {
                        //Challenge the specified user
                        await Context.Channel.SendMessageAsync($"{target.Mention}, you have been challenged to a duel by {Context.User.Mention}\nUse the \"duel [mention target]\" command to accept.");
                    }
                }
            }
            else
            {
                //Tell the current user they have are a dum dum
                await Context.Channel.SendMessageAsync($"{Context.User.Mention}, you cannot duel yourself.");
            }
        }

        [Command("attack")]
        public async Task Attack()
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

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
            
            
        }

        [Command("exitcombat")]
        public async Task ExitCombat()
        {
            var user = UserHandler.GetUser(Context.User.Id);
            
            ContextIds idList = new ContextIds(Context);
            
            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.CharacterExists(idList);
                await UserHandler.ValidCharacterLocation(idList);
            }
            catch(InvalidCharacterStateException)
            {
                return;
            }

            if(user.Char.InPvpCombat)
            {
                var opponent = UserHandler.GetUser(user.Char.InCombatWith);

                user.Char.ExitCombat();
                opponent.Char.ExitCombat();

                await Context.Channel.SendMessageAsync($"{Context.User.Mention} has forfeited the match! {opponent.Char.Name} wins by default.");
            }
            else if(user.Char.InCombat)
            {
                await Context.Channel.SendMessageAsync($"{Context.User.Mention} blacked out!");
            }
            else
            {
                await Context.Channel.SendMessageAsync($"{Context.User.Mention}, you cannot exit combat if you are not in combat.");
            }
            
        }

        [Command("pheal")]
        public async Task Heal()
        {
            ContextIds idList = new ContextIds(Context);
            var user = UserHandler.GetUser(idList.UserId);

            //Tests each case to make sure all circumstances for the execution of this command are valid (character exists, in correct location)
            try
            {
                await UserHandler.CharacterExists(idList);
                await UserHandler.ValidCharacterLocation(idList);
            }
            catch(InvalidCharacterStateException)
            {
                return;
            }

            foreach(BasicMon mon in user.Char.Party)
            {
                mon.Heal();
                foreach(BasicMove move in mon.ActiveMoves)
                {
                    move.Restore();
                }
            }

            await MessageHandler.SendMessage(idList, $"{user.Mention}, your party has been healed!");
        }
    }
}