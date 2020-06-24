using System;

namespace DiscomonProject
{
    public class SeismicChop : BasicMove
    {
        public override string Name { get; } = "Seismic Chop";
        public override string Description { get; } = "The enemy mon is tossed in the air and hit with a karate chop.";
        public override BasicType Type { get; } = new FightingType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 80;
        public override int Accuracy { get; } = 90;
        public override int MaxPP { get; } = 10;
        
        public SeismicChop() :base()
        {

        }

        public SeismicChop(bool newmove) :base(newmove)
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