using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Arness : BasicMon
    {
        public override string Species { get; } = "Arness";
        public override List<BasicType> Typing { get; set; } = new List<BasicType>() {new ShadeType(true)};
        public override string ArtURL { get; } = "https://cdn.discordapp.com/attachments/438843122361827331/722698366084972544/Arness.png";
        public override int BaseHP { get; } = 45;
        public override int BaseAtt { get; } = 75;
        public override int BaseDef { get; } = 70;
        public override int BaseAff { get; } = 40;
        public override int BaseSpd { get; } = 40;
        public override List<int> EvGains { get; } = new List<int>() {0, 1, 1, 0, 0};
        public override int DexNum { get; } = 999;
        public override string DexEntry { get; } = "Arness is forever chained to the ground.";

        public Arness() :base()
        {

        }
        
        public Arness(bool newmon) :base(newmon)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public Arness(int customLvl, List<int> customIvs, List<int> customEvs, string customNature) :base(customLvl, customIvs, customEvs, customNature)
        {
            MoveSetup();
            GenActiveMoves();
        }

        public override void MoveSetup()
        {
            Moveset.Add(new MovesetItem(8, new Headbutt(true)));
            Moveset.Add(new MovesetItem(13, new Disarm(true)));
            Moveset.Add(new MovesetItem(16, new Teleport(true)));
            Moveset.Add(new MovesetItem(22, new NetherPunch(true)));
        }
    }
}