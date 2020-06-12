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
            var moves = "";
            foreach(BasicMove move in mon.ActiveMoves)
            {
                if(move.Name != "None")
                {
                    moves += $"**{move.Name}**, ";    
                }
            }
            moves = moves.Substring(0, moves.Length-2);

            var builder = new EmbedBuilder()
            .WithTitle(mon.TypingToString())
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
            .AddField("Level", mon.Level, true)
            .AddField("HP", $"{mon.CurrentHP}/{mon.TotalHP}", true)
            .AddField("Stats", mon.CurStatsToString())
            .AddField("Moves", moves)
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
	        .WithTitle(mon.TypingToString())
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
            int r = mon.HPGradient()[0];
            int g = mon.HPGradient()[1];
            int b = mon.HPGradient()[2];

            var builder = new EmbedBuilder()
	        .WithTitle($"{user.Name} sends out **{mon.Nickname}**!")
            .WithThumbnailUrl(mon.ArtURL)
        	.WithColor(r, g, b);
            var embed = builder.Build();

            return embed;
        }

        public static Embed FieldMon(BasicMon mon)
        {
            int r = mon.HPGradient()[0];
            int g = mon.HPGradient()[1];
            int b = mon.HPGradient()[2];

            string statuses = "";
            if(mon.Status.Paraylzed)
            {
                statuses += "<:Paralyzed:716427812558602250>";
            }
            if(mon.Status.Burned)
            {
                statuses += "<:Burn:717232618327769141>";
            }

            var builder = new EmbedBuilder()
	        .WithTitle($"Lv. {mon.Level}")
            .WithThumbnailUrl(mon.ArtURL)
        	.WithColor(r, g, b)
            .WithAuthor($"{mon.Nickname} {mon.GenderSymbol}")
            .WithDescription($"{mon.CurrentHP}/{mon.TotalHP} HP {statuses}");
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

        public static Embed MoveScreen(BasicMon mon)
        {
            var builder = new EmbedBuilder()
	        .WithTitle("**Moves**")
        	.WithColor(62, 255, 62)
            .AddField(mon.ActiveMoves[0].Name, mon.ActiveMoves[0].Description + $"\nPP: {mon.ActiveMoves[0].CurrentPP}/{mon.ActiveMoves[0].MaxPP}", true)
            .AddField(mon.ActiveMoves[1].Name, mon.ActiveMoves[1].Description + $"\nPP: {mon.ActiveMoves[1].CurrentPP}/{mon.ActiveMoves[1].MaxPP}", true)
            .AddField(mon.ActiveMoves[2].Name, mon.ActiveMoves[2].Description + $"\nPP: {mon.ActiveMoves[2].CurrentPP}/{mon.ActiveMoves[2].MaxPP}", true)
            .AddField(mon.ActiveMoves[3].Name, mon.ActiveMoves[3].Description + $"\nPP: {mon.ActiveMoves[3].CurrentPP}/{mon.ActiveMoves[3].MaxPP}", true);
            var embed = builder.Build();

            return embed;
        }
    }
}