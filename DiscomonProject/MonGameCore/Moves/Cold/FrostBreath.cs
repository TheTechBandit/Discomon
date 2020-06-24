using System;

namespace DiscomonProject
{
    public class FrostBreath : BasicMove
    {
        public override string Name { get; } = "Frost Breath";
        public override string Description { get; } = "The user breathes out a torrent of freezing cold air towards the opponent, this move is always a critical hit.";
        public override BasicType Type { get; } = new ColdType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 60;
        public override int Accuracy { get; } = 95;
        public override int MaxPP { get; } = 10;
        
        public FrostBreath() :base()
        {

        }

        public FrostBreath(bool newmove) :base(newmove)
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
                dmg = ApplyPowerAlwaysCrit(inst, owner);
                enemy.TakeDamage(dmg);
                if(RandomGen.PercentChance(10.0))
                    Result.StatusMessages.Add(enemy.SetFrozen());
            }
            return Result;
        }
    }
}