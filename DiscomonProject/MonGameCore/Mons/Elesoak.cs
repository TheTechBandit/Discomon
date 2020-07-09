using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Elesoak : BasicMon
    {
        public override string Species { get; } = "Elesoak";
        public override List<BasicType> Typing { get; set; } = new List<BasicType>() {new WaterType(true)};
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/438843122361827331/723260715980554260/elesoak.jpg";
        public override int BaseHP { get; } = 55;
        public override int BaseAtt { get; } = 45;
        public override int BaseDef { get; } = 60;
        public override int BaseAff { get; } = 45;
        public override int BaseSpd { get; } = 35;
        public override List<int> EvGains { get; } = new List<int>() {3, 0, 3, 0, 0};
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "Suki travel in flocks, finding sanctuary in numbers. They communicate with other members by transmitting emotions and posturing.";

        public Elesoak() :base()
        {

        }
        
        public Elesoak(bool newmon) :base(newmon)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public Elesoak(int customLvl, List<int> customIvs, List<int> customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public override void MoveSetup()
        {
            Moveset.Add(new MovesetItem(7, new Drench(true)));
            Moveset.Add(new MovesetItem(25, new Torrent(true)));
            Moveset.Add(new MovesetItem(33, new HydroJet(true)));
        }
    }
}