using System;

namespace DiscomonProject
{
    public class GrassWhip : BasicMove
    {
        public override string Name { get; } = "Grass Whip";
        public override string Description { get; } = "The user attacks its opponent with vines, dealing damage.";
        public override BasicType Type { get; } = new NatureType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 45;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 25;
        
        public GrassWhip() :base()
        {

        }

        public GrassWhip(bool newmove) :base(newmove)
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