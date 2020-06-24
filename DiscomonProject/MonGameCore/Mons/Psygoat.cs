using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Psygoat : BasicMon
    {
        public override string Species { get; } = "Psygoat";
        public override List<BasicType> Typing { get; set; } = new List<BasicType>() {new PsychicType(true)};
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/438843122361827331/723422054535266384/Psygoat.png";
        public override int BaseHP { get; } = 55;
        public override int BaseAtt { get; } = 55;
        public override int BaseDef { get; } = 45;
        public override int BaseAff { get; } = 75;
        public override int BaseSpd { get; } = 60;
        public override List<int> EvGains { get; } = new List<int>() {0, 0, 0, 1, 0};
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "Suki travel in flocks, finding sanctuary in numbers. They communicate with other members by transmitting emotions and posturing.";

        public Psygoat() :base()
        {

        }
        
        public Psygoat(bool newmon) :base(newmon)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public Psygoat(int customLvl, List<int> customIvs, List<int> customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public override void MoveSetup()
        {
            Moveset.Add(new MovesetItem(8, new Headbutt(true)));
            Moveset.Add(new MovesetItem(13, new Lullaby(true)));
            Moveset.Add(new MovesetItem(16, new PsychicBlast(true)));
            //Moveset.Add(new MovesetItem(22, new Karma(true)));
        }
    }
}