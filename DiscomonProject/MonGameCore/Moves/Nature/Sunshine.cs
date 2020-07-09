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
            ResetResult();
            AddResult();

            //Fail logic
            if(SelfMoveFailLogic(owner))
            {
                Result[TargetNum].Fail = true;
                Result[TargetNum].Hit = false;
            }
            //Hit logic
            else
            {
                CurrentPP--;
                Result[TargetNum].Messages.Add(inst.Environment.AttemptSunrise());
            }
            
            return Result;
        }
    }
}