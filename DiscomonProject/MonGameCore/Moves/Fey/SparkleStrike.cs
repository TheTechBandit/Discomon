using System;

namespace DiscomonProject
{
    public class SparkleStrike : BasicMove
    {
        public override string Name { get; } = "Sparkle Strike";
        public override string Description { get; } = "The user conjurs a sparkling comet which smashes into the enemy.";
        public override BasicType Type { get; } = new FeyType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 90;
        public override int Accuracy { get; } = 90;
        public override int MaxPP { get; } = 5;
        
        public SparkleStrike() :base()
        {

        }

        public SparkleStrike(bool newmove) :base(newmove)
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
            }
            return Result;
        }
    }
}