using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Sedimo : BasicMon
    {
        public override string Species { get; } = "Sedimo";
        public override List<BasicType> Typing { get; set; } = new List<BasicType>() {new RockType(true)};
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/438843122361827331/724804477777084476/Sedimo.png";
        public override int BaseHP { get; } = 55;
        public override int BaseAtt { get; } = 55;
        public override int BaseDef { get; } = 60;
        public override int BaseAff { get; } = 50;
        public override int BaseSpd { get; } = 50;
        public override List<int> EvGains { get; } = new List<int>() {0, 0, 1, 0, 0};
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "At it’s heart is a crystal that keeps it alive. Greedy excavators will kill Sedimo for it.";

        public Sedimo() :base()
        {

        }
        
        public Sedimo(bool newmon) :base(newmon)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public Sedimo(int customLvl, List<int> customIvs, List<int> customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public override void MoveSetup()
        {
            Moveset.Add(new MovesetItem(13, new Harden(true)));
            Moveset.Add(new MovesetItem(16, new RockFall(true)));
            Moveset.Add(new MovesetItem(22, new BoulderSlam(true)));
        }
    }
}