using System;

namespace DiscomonProject
{
    public class Zap : BasicMove
    {
        public override string Name { get; } = "Zap";
        public override string Description { get; } = "The user zaps the target, paralyzing them.";
        public override BasicType Type { get; } = new ElectricType(true);
        public override bool Contact { get; } = true;
        public override int MaxPP { get; } = 15;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 100;
        
        public Zap() :base()
        {

        }

        public Zap(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);

            //Fail logic
            if(DefaultFailLogic(enemy, owner) || StatusFailLogic(enemy, "Electric"))
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
                Result.StatusMessages.Add(enemy.SetParalysis());
            }
            return Result;
        }
    }
}