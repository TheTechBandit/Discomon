using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Smoledge : BasicMon
    {
        public override string Species { get; } = "Smoledge";
        public override List<BasicType> Typing { get; set; } = new List<BasicType>() {new FireType(true)};
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/438843122361827331/723258035849527337/smoledge.jpg";
        public override int BaseHP { get; } = 45;
        public override int BaseAtt { get; } = 55;
        public override int BaseDef { get; } = 40;
        public override int BaseAff { get; } = 55;
        public override int BaseSpd { get; } = 45;
        public override List<int> EvGains { get; } = new List<int>() {0, 3, 0, 3, 0};
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "Smoledge’s flame is weak, but when it evolves it gets brighter.";

        public Smoledge() :base()
        {

        }
        
        public Smoledge(bool newmon) :base(newmon)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public Smoledge(int customLvl, List<int> customIvs, List<int> customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public override void MoveSetup()
        {
            Moveset.Add(new MovesetItem(6, new Singe(true)));
            Moveset.Add(new MovesetItem(8, new Curl(true)));
            Moveset.Add(new MovesetItem(12, new FlameSpin(true)));
        }
    }
}