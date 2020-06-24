using System;

namespace DiscomonProject
{
    public class NetherPunch : BasicMove
    {
        public override string Name { get; } = "Nether Punch";
        public override string Description { get; } = "The user swings a punch from the nether dimension.";
        public override BasicType Type { get; } = new ShadeType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 80;
        public override int Accuracy { get; } = 90;
        public override int MaxPP { get; } = 15;
        
        public NetherPunch() :base()
        {

        }

        public NetherPunch(bool newmove) :base(newmove)
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
                if(RandomGen.PercentChance(30.0))
                    enemy.SetFlinching();
            }
            return Result;
        }
    }
}