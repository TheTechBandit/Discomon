using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Heatwave : BasicMove
    {
        public override string Name { get; } = "Heatwave";
        public override string Description { get; } = "The user heats up the area, significantly raising the temperature. This boosts the power of fire type moves and decreases the power of water type moves.";
        public override BasicType Type { get; } = new FireType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 5;
        public override string TargetType { get; } = "None";
        
        public Heatwave() :base()
        {

        }

        public Heatwave(bool newmove) :base(newmove)
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
                Result[TargetNum].Messages.Add(inst.Environment.AttemptHeatwave());
            }
            
            return Result;
        }
    }
}