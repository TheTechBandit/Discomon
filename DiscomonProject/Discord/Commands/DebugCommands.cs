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
    public class DebugCommands : ModuleBase<SocketCommandContext>
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

        [Command("debuginfo")]
        public async Task DebugInfo(SocketGuildUser target)
        {
            ContextIds idList = new ContextIds(Context);
            UserAccount user;

            if(target != null)
            {
                user = UserHandler.GetUser(target.Id);
            }
            else
            {
                user = UserHandler.GetUser(idList.UserId);
            }

            await MessageHandler.SendMessage(idList, user.DebugString());
        }

        [Command("debugresetchar")]
        public async Task DebugResetCharacter()
        {
            ContextIds ids = new ContextIds(Context);
            var user = UserHandler.GetUser(ids.UserId);
            user.Char = null;
            user.HasCharacter = false;
            user.PromptState = -1;

            await MessageHandler.SendMessage(ids, $"{user.Mention}, your character has been deleted.");
        }

        [Command("datawipe")]
        public async Task DataWipe()
        {
            ContextIds idList = new ContextIds(Context);
            await MessageHandler.SendMessage(idList, "User data cleared. Reboot bot to take effect.");
            UserHandler.ClearUserData();
            NewCombatHandler.ClearCombatData();
        }

        [Command("whisper")]
        public async Task Whisper()
        {
            ContextIds idList = new ContextIds(Context);

            await MessageHandler.MoveScreen(idList.UserId);
        }

        [Command("emojitest")]
        public async Task EmojiTest()
        {
            ContextIds idList = new ContextIds(Context);

            await MessageHandler.EmojiTest(idList);
        }

        [Command("typetest")]
        public async Task TypeTest()
        {
            ContextIds idList = new ContextIds(Context);

            var attack = new WaterType(true);
            List<BasicType> defense = new List<BasicType>()
            {
                new WaterType(true),
                new FireType(true)
            };

            var effect = attack.ParseEffectiveness(defense);

            string defstr = $"{defense[0].Type}";
            if(defense.Count > 1)
                defstr += $"/{defense[1].Type}";

            await MessageHandler.SendMessage(idList, $"{attack.Type} is {effect}x effective against {defstr}");
        }

        [Command("quickstart")]
        public async Task QuickStart([Remainder]string text)
        {
            ContextIds ids = new ContextIds(Context);
            var user = UserHandler.GetUser(ids.UserId);

            user.Char = new Character(true);
            user.Char.CurrentGuildId = ids.GuildId;
            user.Char.CurrentGuildName = Context.Guild.Name;
            user.Char.Name = user.Name;

            text = text.ToLower();

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

            user.PromptState = -1;
        }

    }
}