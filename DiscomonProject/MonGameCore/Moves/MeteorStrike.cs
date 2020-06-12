using System;

namespace DiscomonProject
{
    public class MeteorStrike : BasicMove
    {
        public override string Name { get; } = "Meteor Strike";
        public override string Description { get; } = "The user calls down a meteor, dealing massive damage.";
        public override BasicType Type { get; } = new FireType(true);
        public override bool Contact { get; } = false;
        public override int MaxPP { get; } = 5;
        public override int Power { get; } = 90;
        public override int Accuracy { get; } = 50;
        
        public MeteorStrike() :base()
        {

        }

        public MeteorStrike(bool newmove) :base(newmove)
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