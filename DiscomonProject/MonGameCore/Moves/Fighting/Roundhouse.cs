using System;

namespace DiscomonProject
{
    public class Roundhouse : BasicMove
    {
        public override string Name { get; } = "Roundhouse";
        public override string Description { get; } = "The user spins around before kicking the target in the face, this intimidates them, sharply lowering their attack";
        public override BasicType Type { get; } = new FightingType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 70;
        public override int Accuracy { get; } = 70;
        public override int MaxPP { get; } = 15;
        
        public Roundhouse() :base()
        {

        }

        public Roundhouse(bool newmove) :base(newmove)
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
                (double mod, string mess) = enemy.ChangeAttStage(-2);
                Result.StatChangeMessages.Add(mess);
            }
            return Result;
        }
    }
}