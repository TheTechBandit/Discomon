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

            await ReplyAsync($"Here is your information: \nID: {acc.UserId}\nName: {acc.Name}\nAvatar: {acc.AvatarUrl}\nGuildID: {acc.Char.CurrentGuildId}");
        }

        [Command("monstat")]
        public async Task MonStat(int num)
        {
            ContextIds ids = new ContextIds(Context);
            UserAccount user = UserHandler.GetUser(ids.UserId);
            
             await Context.Channel.SendMessageAsync(
            "",
            embed: MonEmbedBuilder.MonStats((BasicMon)user.Char.Party[0]))
            .ConfigureAwait(false);
        }

        [Command("debuginfo")]
        public async Task DebugInfo(SocketGuildUser target)
        {
            ContextIds ids = new ContextIds(Context);
            UserAccount user;

            if(target != null)
            {
                user = UserHandler.GetUser(target.Id);
            }
            else
            {
                user = UserHandler.GetUser(ids.UserId);
            }

            await MessageHandler.SendMessage(ids.GuildId, ids.ChannelId, user.DebugString());
        }

        [Command("debugreset")]
        public async Task DebugReset()
        {
            ContextIds ids = new ContextIds(Context);
            var user = UserHandler.GetUser(ids.UserId);
            user.Char = null;
            user.HasCharacter = false;
            user.PromptState = -1;

            await MessageHandler.SendMessage(ids, $"{user.Mention}, your character has been reset.");
        }

        [Command("enter")]
        public async Task Enter([Remainder]string text)
        {
            ContextIds ids = new ContextIds(Context);
            var user = UserHandler.GetUser(ids.UserId);
            var originalText = text;
            text = text.ToLower();

            /* PROMPT STATE MEANINGS-
            -1- Has no character
            0- Awaiting confirmation or cancellation of character creation
            1- Character creation confirmed. Awaiting name.
            2- Name confirmed. Awaiting partner.
            */
            switch(user.PromptState)
            {
                case 0:
                    if(text.Equals("confirm"))
                    {
                        user.Char = new Character(true);
                        user.Char.CurrentGuildId = ids.GuildId;
                        user.Char.CurrentGuildName = Context.Guild.Name;
                        user.PromptState = 1;
                        await MessageHandler.SendMessage(ids, $"Beginning character creation for {user.Mention}.\nWhat is your name? (use the \"enter\" command to enter your name)");
                    }
                    else if(text.Equals("cancel"))
                    {
                        user.PromptState = -1;
                        await MessageHandler.SendMessage(ids, $"Character creation cancelled for {user.Mention}.");
                    }
                    else
                    {
                        await MessageHandler.SendMessage(ids, $"{user.Mention}, I'm sorry, but I don't recognize that. Please enter \"confirm\" or \"cancel\"");
                    }
                    break;

                case 1:
                    if(text.Length <= 32 && text.Length > 0)
                    {
                        user.Char.Name = originalText;
                        user.PromptState = 2;
                        await MessageHandler.SendMessage(ids, $"{user.Mention}, your character's name is now {originalText}. Now you must choose your partner.");

                        await Context.Channel.SendMessageAsync(
                        "", embed: MonEmbedBuilder.MonDex(new Snoril(true)))
                        .ConfigureAwait(false);

                        await Context.Channel.SendMessageAsync(
                        "", embed: MonEmbedBuilder.MonDex(new Suki(true)))
                        .ConfigureAwait(false);
                    }
                    else
                    {
                        await MessageHandler.SendMessage(ids, $"{user.Mention}, your name must be 32 characters or less.");
                    }
                    break;

                case 2:
                    if(text.Equals("snoril") || text.Equals("1"))
                    {
                        user.Char.Party.Add(new Snoril(true));
                        user.HasCharacter = true;
                        await MessageHandler.SendMessage(ids, $"{user.Mention}, you have chosen Snoril as your partner! Good luck on your adventure.");
                    }
                    else if(text.Equals("suki") || text.Equals("2"))
                    {
                        user.Char.Party.Add(new Suki(true));
                        user.HasCharacter = true;
                        await MessageHandler.SendMessage(ids, $"{user.Mention}, you have chosen Suki as your partner! Good luck on your adventure.");
                    }
                    else
                    {
                        await MessageHandler.SendMessage(ids, $"{user.Mention}, please enter either Snoril or Suki.");
                    }
                    break;
            }
        }

        [Command("startadventure")]
        public async Task StartAdventure()
        {
            ContextIds ids = new ContextIds(Context);
            var user = UserHandler.GetUser(ids.UserId);

            if((user.PromptState == -1 || user.PromptState == 0) && !user.HasCharacter)
            {
                user.PromptState = 0;
                await MessageHandler.SendMessage(ids, $"{user.Mention}, are you sure you want to create a character here? You can only have one and it will be locked to this particular location. Moving to a new location will take time and money. Type the \"enter confirm\" comnmand again to confirm character creation or \"enter cancel\" to cancel.");
            }
            else if(user.HasCharacter)
            {
                await MessageHandler.SendMessage(ids, $"{user.Mention}, you already have a character!");
            }
            else
            {
                await MessageHandler.SendMessage(ids, $"{user.Mention}, you are already in the process of creating a character!");
            }
        }

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
                        await Context.Channel.SendMessageAsync($"The duel between {target.Mention} and {Context.User.Mention} will now begin!");
                        fromUser.Char.InCombat = true;
                        fromUser.Char.InPvpCombat = true;
                        fromUser.Char.CombatRequest = 0;
                        fromUser.Char.InCombatWith = toUser.UserId;

                        toUser.Char.InCombat = true;
                        toUser.Char.InPvpCombat = true;
                        toUser.Char.CombatRequest = 0;
                        toUser.Char.InCombatWith = fromUser.UserId;
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
                user.Char.InCombat = false;
                user.Char.InPvpCombat = false;
                user.Char.InCombatWith = 0;

                opponent.Char.InCombat = false;
                opponent.Char.InPvpCombat = false;
                opponent.Char.InCombatWith = 0;

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
    }
}