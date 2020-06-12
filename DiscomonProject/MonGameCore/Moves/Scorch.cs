using System;

namespace DiscomonProject
{
    public class Scorch : BasicMove
    {
        public override string Name { get; } = "Scorch";
        public override string Description { get; } = "The user scorches the target, burning them.";
        public override BasicType Type { get; } = new FireType(true);
        public override bool Contact { get; } = false;
        public override int MaxPP { get; } = 15;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 100;
        
        public Scorch() :base()
        {

        }

        public Scorch(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);

            //Fail logic
            if(DefaultFailLogic(enemy, owner) || StatusFailLogic(enemy, "Fire"))
            {
                Result.Fail = true;
                Result.Hit = false;
            }
            else if(!ApplyAccuracy(inst, owner))
            {
                Result.Miss = true;
                Result.Hit = false;
            }
            //Hit logic
            else
            {
                CurrentPP--;
                Result.StatusMessages.Add(enemy.SetBurned());
            }
            return Result;
        }
    }
}