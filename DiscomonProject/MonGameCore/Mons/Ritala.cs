using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Ritala : BasicMon
    {
        public override string Species { get; } = "Ritala";
        public override List<BasicType> Typing { get; set; } = new List<BasicType>() {new AirType(true)};
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/438843122361827331/722877717086994532/unknown.png";
        public override int BaseHP { get; } = 70;
        public override int BaseAtt { get; } = 60;
        public override int BaseDef { get; } = 55;
        public override int BaseAff { get; } = 35;
        public override int BaseSpd { get; } = 65;
        public override List<int> EvGains { get; } = new List<int>() {2, 0, 0, 0, 0};
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "Ritala Ritala Ritala Ritala Ritala";

        public Ritala() :base()
        {

        }
        
        public Ritala(bool newmon) :base(newmon)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public Ritala(int customLvl, List<int> customIvs, List<int> customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public override void MoveSetup()
        {
            Moveset.Add(new MovesetItem(13, new Gust(true)));
            Moveset.Add(new MovesetItem(19, new Bite(true)));
            Moveset.Add(new MovesetItem(23, new EvasiveManeuvers(true)));
            Moveset.Add(new MovesetItem(32, new DiveBomb(true)));
        }
    }
}