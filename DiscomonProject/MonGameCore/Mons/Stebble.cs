using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Stebble : BasicMon
    {
        public override string Species { get; } = "Stebble";
        public override List<BasicType> Typing { get; set; } = new List<BasicType>() {new MetalType(true)};
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/438843122361827331/723426681179603004/Stebble.png";
        public override int BaseHP { get; } = 60;
        public override int BaseAtt { get; } = 50;
        public override int BaseDef { get; } = 65;
        public override int BaseAff { get; } = 40;
        public override int BaseSpd { get; } = 35;
        public override List<int> EvGains { get; } = new List<int>() {1, 0, 1, 0, 0};
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "Suki travel in flocks, finding sanctuary in numbers. They communicate with other members by transmitting emotions and posturing.";

        public Stebble() :base()
        {

        }
        
        public Stebble(bool newmon) :base(newmon)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public Stebble(int customLvl, List<int> customIvs, List<int> customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public override void MoveSetup()
        {
            Moveset.Add(new MovesetItem(6, new IronCharge(true)));
            Moveset.Add(new MovesetItem(8, new Forge(true)));
            Moveset.Add(new MovesetItem(12, new Gear(true)));
            Moveset.Add(new MovesetItem(16, new Slice(true)));
        }
    }
}