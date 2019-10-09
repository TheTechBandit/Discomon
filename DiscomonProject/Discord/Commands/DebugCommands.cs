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
    }
}