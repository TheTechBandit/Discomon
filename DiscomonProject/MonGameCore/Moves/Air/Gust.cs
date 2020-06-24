using System;

namespace DiscomonProject
{
    public class Gust : BasicMove
    {
        public override string Name { get; } = "Gust";
        public override string Description { get; } = "The user beats its wings, sending a gust of air at the opponent, dealing damage.";
        public override BasicType Type { get; } = new AirType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 50;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 30;
        
        public Gust() :base()
        {

        }

        public Gust(bool newmove) :base(newmove)
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
            //Hit Logic
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