using System;

namespace DiscomonProject
{
    public class IronCharge : BasicMove
    {
        public override string Name { get; } = "Iron Charge";
        public override string Description { get; } = "The user charges the enemy blindly. This move will lower the defense of the user.";
        public override BasicType Type { get; } = new MetalType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 100;
        public override int Accuracy { get; } = 90;
        public override int MaxPP { get; } = 5;
        
        public IronCharge() :base()
        {

        }

        public IronCharge(bool newmove) :base(newmove)
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
                (double mod, string mess) = owner.ChangeDefStage(-1);
                Result.StatChangeMessages.Add(mess);
            }
            return Result;
        }
    }
}