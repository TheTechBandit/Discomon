using System;
using Discord;

namespace DiscomonProject.Discord
{
    public static class MonEmbedBuilder
    {
        static MonEmbedBuilder()
        {

        }

        public static Embed MonStats(BasicMon mon)
        {
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

            return embed;
        }
    }
}