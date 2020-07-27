using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Suki : BasicMon
    {
        public override string Species { get; } = "Suki";
        public override List<BasicType> Typing { get; set; } = new List<BasicType>() {new ColdType(true), new AirType(true)};
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/516760928423772163/601482394045775882/suki.png";
        public override int BaseHP { get; } = 60;
        public override int BaseAtt { get; } = 70;
        public override int BaseDef { get; } = 55;
        public override int BaseAff { get; } = 70;
        public override int BaseSpd { get; } = 50;
        public override List<int> EvGains { get; } = new List<int>() {0, 1, 0, 0, 1};
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "Suki travel in flocks, finding sanctuary in numbers. They communicate with other members by transmitting emotions and posturing.";

        public Suki() :base()
        {

        }
        
        public Suki(bool newmon) :base(newmon)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public Suki(int customLvl, List<int> customIvs, List<int> customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public override void MoveSetup()
        {
            Moveset.Add(new MovesetItem(19, new IceFang(true)));
            Moveset.Add(new MovesetItem(23, new FrostBreath(true)));
            Moveset.Add(new MovesetItem(32, new MeteorStrike(true)));
        }
    }
}