using System;

namespace DiscomonProject
{
    public class Singe : BasicMove
    {
        public override string Name { get; } = "Singe";
        public override string Description { get; } = "The user attacks its opponent with heat, dealing damage and potentially burning them.";
        public override BasicType Type { get; } = new FireType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 40;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 25;
        
        public Singe() :base()
        {

        }

        public Singe(bool newmove) :base(newmove)
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
                if(RandomGen.PercentChance(10.0))
                    Result.StatusMessages.Add(enemy.SetBurned());
            }
            return Result;
        }
    }
}