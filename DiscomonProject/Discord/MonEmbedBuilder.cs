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
                    .WithText($"Caught By {UserHandler.GetUser(mon.CatcherID).Name}");
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

        public static Embed MonDex(BasicMon mon)
        {
            var builder = new EmbedBuilder()
	        .WithTitle(mon.Typing)
        	.WithColor(139, 87, 42)
            .WithFooter(footer => {
                footer
                    .WithText($"Dex No. {mon.DexNum}");
            })
            .WithImageUrl(mon.ArtURL)
            .WithAuthor(author => {
                author
                    .WithName(mon.Species)
                    .WithIconUrl(mon.ArtURL);
            })
            .AddField("Stats", mon.BaseStatsToString())
            .AddField("Dex Entry", mon.DexEntry);
            var embed = builder.Build();

            return embed;
        }

        public static Embed MonSendOut(UserAccount user, BasicMon mon)
        {
            var builder = new EmbedBuilder()
	        .WithTitle($"{user.Name} sends out **{mon.Nickname}**!")
            .WithThumbnailUrl(mon.ArtURL)
        	.WithColor(255, 62, 62);
            var embed = builder.Build();

            return embed;
        }

        public static Embed FieldMon(BasicMon mon)
        {
            var builder = new EmbedBuilder()
	        .WithTitle($"Lv. {mon.Level}")
            .WithThumbnailUrl(mon.ArtURL)
        	.WithColor(255, 62, 62)
            .WithAuthor($"{mon.Nickname} {mon.GenderSymbol}")
            .WithDescription($"{mon.CurrentHP}/{mon.TotalHP} HP");
            var embed = builder.Build();

            return embed;
        }

        public static Embed EmptyPartySpot(int num)
        {
            string spots = "";
            for(int i = 0; i < num; i++)
            {
                spots += "[ Empty ]\n\n";
            }

            var builder = new EmbedBuilder()
	        .WithTitle(spots)
        	.WithColor(62, 255, 62);
            var embed = builder.Build();

            return embed;
        }

        public static Embed FightScreen(BasicMon mon)
        {
            var builder = new EmbedBuilder()
	        .WithTitle(mon.Nickname)
        	.WithColor(62, 255, 62)
            .WithThumbnailUrl(mon.ArtURL)
            .WithImageUrl("https://cdn.discordapp.com/attachments/452818546175770624/626432333414924288/fight_screen.png");
            var embed = builder.Build();

            return embed;
        }
    }
}