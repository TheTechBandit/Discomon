using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using DiscomonProject.Storage.Implementations;
using Discord.Commands;

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
    }
}