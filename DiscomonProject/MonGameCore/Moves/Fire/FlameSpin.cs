using System;

namespace DiscomonProject
{
    public class FlameSpin : BasicMove
    {
        public override string Name { get; } = "Flame Spin";
        public override string Description { get; } = "The user spins very quickly before rolling into the opponent, dealing damage and raising its own speed.";
        public override BasicType Type { get; } = new FireType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 50;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 25;
        
        public FlameSpin() :base()
        {

        }

        public FlameSpin(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);
            int dmg = 0;

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
                dmg = ApplyPower(inst, owner);
                enemy.TakeDamage(dmg);
                (double mod, string mess) = owner.ChangeSpdStage(1);
                Result.StatChangeMessages.Add(mess);
            }
            return Result;
        }
    }
}