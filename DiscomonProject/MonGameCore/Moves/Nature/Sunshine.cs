using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Sunshine : BasicMove
    {
        public override string Name { get; } = "Sunshine";
        public override string Description { get; } = "The sun shines more brightly, increasing the power of nature type attacks.";
        public override BasicType Type { get; } = new NatureType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 10;
        public override string TargetType { get; } = "None";
        
        public Sunshine() :base()
        {

        }

        public Sunshine(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            Console.WriteLine("SA");
            ResetResult();
            Console.WriteLine("SB");
            AddResult();
            Console.WriteLine("SC");

            //Fail logic
            if(SelfMoveFailLogic(owner))
            {
                Console.WriteLine("SD");
                Result[TargetNum].Fail = true;
                Console.WriteLine("SE");
                Result[TargetNum].Hit = false;
                Console.WriteLine("SF");
            }
            //Hit logic
            else
            {
                Console.WriteLine("SG");
                CurrentPP--;
                Console.WriteLine("SH");
                Result[TargetNum].Messages.Add(inst.Environment.AttemptSunrise());
                Console.WriteLine("SI");
            }
            
            return Result;
        }
    }
}