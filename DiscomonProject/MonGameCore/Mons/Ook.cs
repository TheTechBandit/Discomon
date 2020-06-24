using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Ook : BasicMon
    {
        public override string Species { get; } = "Ook";
        public override List<BasicType> Typing { get; set; } = new List<BasicType>() {new FightingType(true)};
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/438843122361827331/723424724209893437/Ook.png";
        public override int BaseHP { get; } = 45;
        public override int BaseAtt { get; } = 65;
        public override int BaseDef { get; } = 45;
        public override int BaseAff { get; } = 40;
        public override int BaseSpd { get; } = 55;
        public override List<int> EvGains { get; } = new List<int>() {0, 1, 0, 0, 1};
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "The primal monkey mon. Ook can smell berries that trainers have tucked away and will fight for them.";

        public Ook() :base()
        {

        }
        
        public Ook(bool newmon) :base(newmon)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public Ook(int customLvl, List<int> customIvs, List<int> customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public override void MoveSetup()
        {
            Moveset.Add(new MovesetItem(8, new Bite(true)));
            Moveset.Add(new MovesetItem(13, new Roundhouse(true)));
            Moveset.Add(new MovesetItem(16, new SeismicChop(true)));
            Moveset.Add(new MovesetItem(22, new Meditate(true)));
        }
    }
}