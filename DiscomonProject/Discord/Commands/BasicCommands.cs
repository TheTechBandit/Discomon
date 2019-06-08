using System.Collections.Generic;
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
            DiscordUser user = new DiscordUser(Context.User);
            UserAccount acc = UserHandler.GetUser(user);

            await ReplyAsync($"Here is your information: \nID: {acc.getID()}\nName: {acc.name}");
        }
    }
}