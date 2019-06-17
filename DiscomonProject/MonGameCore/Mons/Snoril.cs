using System.Collections;

namespace DiscomonProject
{
    public class Snoril : BasicMon
    {
        public override string Species { get; } = "Snoril";
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/452818546175770624/589914095390818315/snail.png";
        public override int BaseHP { get; } = 90;
        public override int BaseAtt { get; } = 65;
        public override int BaseDef { get; } = 65;
        public override int BaseAff { get; } = 85;
        public override int BaseSpd { get; } = 15;
        public override ArrayList EvGains { get; } = new ArrayList{1, 0, 0, 1, 0};
        public override string Typing { get; set; } = "Beast";
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "Slowpoke uses its tail to catch prey by dipping it in water at the side of a river. However, this Pokémon often forgets what it’s doing and often spends entire days just loafing at water’s edge.";


        public Snoril() : base()
        {

        }

        public Snoril(int customLvl, ArrayList customIvs, ArrayList customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {

        }
    }
}