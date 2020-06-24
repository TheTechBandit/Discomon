using System;

namespace DiscomonProject
{
    public class Slice : BasicMove
    {
        public override string Name { get; } = "Slice";
        public override string Description { get; } = "The user slashes at the enemy, dealing damage.";
        public override BasicType Type { get; } = new MetalType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 40;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 35;
        
        public Slice() :base()
        {

        }

        public Slice(bool newmove) :base(newmove)
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