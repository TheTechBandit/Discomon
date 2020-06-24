using System;

namespace DiscomonProject
{
    public class Bite : BasicMove
    {
        public override string Name { get; } = "Bite";
        public override string Description { get; } = "The user chomps down on the opponent.";
        public override BasicType Type { get; } = new BeastType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 60;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 25;
        
        public Bite() :base()
        {

        }

        public Bite(bool newmove) :base(newmove)
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