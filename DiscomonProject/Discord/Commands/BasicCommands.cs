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
    public class BasicCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }

        [Command("whoami")]
        public async Task ShowAccount()
        {
            UserAccount acc = UserHandler.GetUser(Context.User.Id);

            await ReplyAsync($"Here is your information: \nID: {acc.UserId}\nName: {acc.Name}\nAvatar: {acc.AvatarUrl}\nGuildID: {acc.CurrentGuildId}");
        }

        [Command("monstat")]
        public async Task MonStat()
        {
            BasicMon mon = new Snoril(20, new ArrayList{17, 5, 13, 16, 14}, new ArrayList{0, 0, 0, 0, 0}, "Hasty");

             await Context.Channel.SendMessageAsync(
            "",
            embed: MonEmbedBuilder.MonStats(mon))
            .ConfigureAwait(false);
        }

        [Command("duel")]
        public async Task Duel(SocketGuildUser target)
        {
            var fromUser = UserHandler.GetUser(Context.User.Id);
            var toUser = UserHandler.GetUser(target.Id);

            //Check that the user did not target themself with the command
            if(fromUser.UserId != toUser.UserId)
            {
                //Set the current user's combat request ID to the user specified
                fromUser.CombatRequest = toUser.UserId;

                //Check if the specified user has a combat request ID that is the current user's ID
                if(toUser.CombatRequest == fromUser.UserId)
                {
                    //Make sure neither users are in combat while sending response request
                    if(fromUser.InCombat)
                    {
                        await Context.Channel.SendMessageAsync($"{Context.User.Mention}, you cannot start a duel while in combat!");
                    }
                    else if(toUser.InCombat)
                    {
                        await Context.Channel.SendMessageAsync($"{Context.User.Mention}, you cannot start a duel with a player who is in combat!");
                    }
                    else
                    {
                        //Start duel
                        await Context.Channel.SendMessageAsync($"The duel between {target.Mention} and {Context.User.Mention} will now begin!");
                        fromUser.InCombat = true;
                        toUser.InCombat = true;
                    }
                }
                else
                {
                    //Make sure neither users are in combat while sending initial request
                    if(fromUser.InCombat)
                    {
                        await Context.Channel.SendMessageAsync($"{Context.User.Mention}, you cannot request a duel when you are in combat!");
                    }
                    else if(toUser.InCombat)
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
    }
}