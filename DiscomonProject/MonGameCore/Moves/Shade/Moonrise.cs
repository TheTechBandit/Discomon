using System;

namespace DiscomonProject
{
    public class Moonrise : BasicMove
    {
        public override string Name { get; } = "Moonrise";
        public override string Description { get; } = "The user performs a ritual, raising the moon and darkening the area.";
        public override BasicType Type { get; } = new ShadeType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 5;
        
        public Moonrise() :base()
        {

        }

        public Moonrise(bool newmove) :base(newmove)
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
                Result.Messages.Add(inst.Environment.AttemptMoonrise());
            }
            return Result;
        }
    }
}