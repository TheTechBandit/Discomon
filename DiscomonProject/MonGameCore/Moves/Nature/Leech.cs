using System;

namespace DiscomonProject
{
    public class Leech : BasicMove
    {
        public override string Name { get; } = "Leech";
        public override string Description { get; } = "The user drains a small amount of HP from the opposing mon, damaging the opponent and healing itself.";
        public override BasicType Type { get; } = new NatureType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 90;
        public override int MaxPP { get; } = 10;
        
        public Leech() :base()
        {

        }

        public Leech(bool newmove) :base(newmove)
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
                var amount = enemy.TotalHP/6;
                owner.Restore(amount);
                enemy.TakeDamage(amount);

                Result.SelfHeal = amount;
                Result.EnemyDmg = amount;
            }
            return Result;
        }
    }
}