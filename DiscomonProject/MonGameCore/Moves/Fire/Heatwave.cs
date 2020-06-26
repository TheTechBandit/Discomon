using System;

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
        
        public Heatwave() :base()
        {

        }

        public Heatwave(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);

            //Fail logic
            if(DefaultFailLogic(enemy, owner))
            {
                Result.Fail = true;
                Result.Hit = false;
            }
            //Miss Logic
            else if(!ApplyAccuracy(inst, owner))
            {
                Result.Miss = true;
                Result.Hit = false;
            }
            //Hit logic
            else
            {
                CurrentPP--;
                Result.Messages.Add(inst.Environment.AttemptHeatwave());
            }
            return Result;
        }
    }
}