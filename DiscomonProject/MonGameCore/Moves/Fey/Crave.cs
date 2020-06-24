using System;

namespace DiscomonProject
{
    public class Crave : BasicMove
    {
        public override string Name { get; } = "Crave";
        public override string Description { get; } = "The opponent is hit with a sudden craving for a snack! This lowers the opponenet's defense.";
        public override BasicType Type { get; } = new FeyType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 40;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 25;
        
        public Crave() :base()
        {

        }

        public Crave(bool newmove) :base(newmove)
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
                (double mod, string mess) = enemy.ChangeDefStage(-1);
                Result.StatChangeMessages.Add(mess);
            }
            return Result;
        }
    }
}