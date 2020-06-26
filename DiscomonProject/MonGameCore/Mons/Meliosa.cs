using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Meliosa : BasicMon
    {
        public override string Species { get; } = "Meliosa";
        public override List<BasicType> Typing { get; set; } = new List<BasicType>() {new FeyType(true)};
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/438843122361827331/722737219743514686/Meliosa.png";
        public override int BaseHP { get; } = 40;
        public override int BaseAtt { get; } = 50;
        public override int BaseDef { get; } = 45;
        public override int BaseAff { get; } = 70;
        public override int BaseSpd { get; } = 50;
        public override List<int> EvGains { get; } = new List<int>() {0, 0, 0, 2, 0};
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "Meliosa can change the weather on a whim.";

        public Meliosa() :base()
        {

        }
        
        public Meliosa(bool newmon) :base(newmon)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public Meliosa(int customLvl, List<int> customIvs, List<int> customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public override void MoveSetup()
        {
            Moveset.Add(new MovesetItem(8, new Crave(true)));
            Moveset.Add(new MovesetItem(13, new Forgiveness(true)));
            Moveset.Add(new MovesetItem(16, new SparkleStrike(true)));
            Moveset.Add(new MovesetItem(22, new Wish(true)));
            //Moveset.Add(new MovesetItem(30, new Sunshine(true)));
            //Moveset.Add(new MovesetItem(30, new Heatwave(true)));
        }
    }
}