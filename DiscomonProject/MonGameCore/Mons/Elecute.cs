using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Elecute : BasicMon
    {
        public override string Species { get; } = "Elecute";
        public override List<BasicType> Typing { get; set; } = new List<BasicType>() {new ElectricType(true)};
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/438843122361827331/723274102499967046/Elecute.jpg";
        public override int BaseHP { get; } = 45;
        public override int BaseAtt { get; } = 60;
        public override int BaseDef { get; } = 40;
        public override int BaseAff { get; } = 65;
        public override int BaseSpd { get; } = 40;
        public override List<int> EvGains { get; } = new List<int>() {0, 0, 0, 1, 0};
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "Its fur is constantly generating large amounts of static electricity. At first glance, many trainers believe they are highly affectionate. In fact, they are highly aggressive and rubbing against you means it is trying to kill you.";

        public Elecute() :base()
        {

        }
        
        public Elecute(bool newmon) :base(newmon)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public Elecute(int customLvl, List<int> customIvs, List<int> customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public override void MoveSetup()
        {
            Moveset.Add(new MovesetItem(6, new Zap(true)));
            Moveset.Add(new MovesetItem(7, new ChargeUp(true)));
            Moveset.Add(new MovesetItem(25, new StaticShock(true)));
            Moveset.Add(new MovesetItem(33, new Lightning(true)));
        }
    }
}