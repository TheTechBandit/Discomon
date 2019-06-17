using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using DiscomonProject.Storage.Implementations;
using Discord;
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

        [Command("testembed")]
        public async Task TestEmbed()
        {
            BasicMon mon = new Snoril(20, new ArrayList{17, 5, 13, 16, 14}, new ArrayList{0, 0, 0, 0, 0}, "Hasty");

            var builder = new EmbedBuilder()
	        .WithTitle(mon.Typing)
        	.WithDescription($"DEX NO. {mon.DexNum}")
        	.WithColor(139, 87, 42)
        	.WithTimestamp(DateTimeOffset.FromUnixTimeMilliseconds(1560726232528))
            .WithFooter(footer => {
                footer
                    .WithText($"Caught By {mon.Catcher}");
            })
            .WithImageUrl(mon.ArtURL)
            .WithAuthor(author => {
                author
                    .WithName(mon.Species)
                    .WithIconUrl(mon.ArtURL);
            })
            .AddField("Level", mon.Level)
            .AddField("HP", $"{mon.CurrentHP}/{mon.TotalHP}")
            .AddField("Stats", mon.CurStatsToString())
            .AddField("Moves", "**NOT IMPLEMENTED YET**")
            .AddField("Nature", mon.Nature, true)
            .AddField("Ability", "**NOT IMPLEMENTED YET**", true)
            .AddField("Held Item", "**NOT IMPLEMENTED YET**", true)
            .AddField("Gender", mon.Gender, true)
            .AddField("IVs", mon.IvsToString())
            .AddField("EVs", mon.EvsToString());
           var embed = builder.Build();
             await Context.Channel.SendMessageAsync(
            "",
            embed: embed)
            .ConfigureAwait(false);
        }
    }
}