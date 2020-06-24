using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Grasipup : BasicMon
    {
        public override string Species { get; } = "Grasipup";
        public override List<BasicType> Typing { get; set; } = new List<BasicType>() {new NatureType(true)};
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/438843122361827331/722316266676682822/Grasipup.png";
        public override int BaseHP { get; } = 50;
        public override int BaseAtt { get; } = 55;
        public override int BaseDef { get; } = 45;
        public override int BaseAff { get; } = 35;
        public override int BaseSpd { get; } = 55;
        public override List<int> EvGains { get; } = new List<int>() {0, 3, 0, 0, 3};
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "Calm and patient, Grasipup loves to lie in the grass, soaking up the sun. Grasipup doesn't eat and gains all it's energy through an advanced version of photosynthesis using the leaf-like structures on it's body.";

        public Grasipup() :base()
        {

        }
        
        public Grasipup(bool newmon) :base(newmon)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public Grasipup(int customLvl, List<int> customIvs, List<int> customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public override void MoveSetup()
        {
            Moveset.Add(new MovesetItem(6, new GrassWhip(true)));
            Moveset.Add(new MovesetItem(8, new Bite(true)));
            Moveset.Add(new MovesetItem(12, new Leech(true)));
        }
    }
}