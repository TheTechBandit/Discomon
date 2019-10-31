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

        [Command("monstat")]
        public async Task MonStat([Remainder]int num)
        {
            ContextIds idList = new ContextIds(Context);
            UserAccount user = UserHandler.GetUser(idList.UserId);

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
            
            await Context.Channel.SendMessageAsync(
            "",
            embed: MonEmbedBuilder.MonStats((BasicMon)user.Char.Party[num-1]))
            .ConfigureAwait(false);
        }

        [Command("party")]
        public async Task Party()
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

            var count = 6;
            foreach(BasicMon mon in user.Char.Party)
            {
                await MessageHandler.SendEmbedMessage(idList, "", MonEmbedBuilder.FieldMon(mon));
                count--;
            }

            await MessageHandler.SendEmbedMessage(idList, "", MonEmbedBuilder.EmptyPartySpot(count));
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
                        user.Char.Party.Add(new Snoril(true)
                        {
                            CatcherID = user.UserId,
                            OwnerID = user.UserId
                        });
                        user.HasCharacter = true;
                        await MessageHandler.SendMessage(ids, $"{user.Mention}, you have chosen Snoril as your partner! Good luck on your adventure.");
                    }
                    else if(text.Equals("suki") || text.Equals("2"))
                    {
                        user.Char.Party.Add(new Suki(true)
                        {
                            CatcherID = user.UserId,
                            OwnerID = user.UserId
                        });
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

    }
}