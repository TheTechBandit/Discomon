using System;

namespace DiscomonProject
{
    public class Forgiveness : BasicMove
    {
        public override string Name { get; } = "Forgiveness";
        public override string Description { get; } = "The user forgives the opponent, healing the opponent and sharply lowering their attack.";
        public override BasicType Type { get; } = new FeyType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 10;
        
        public Forgiveness() :base()
        {

        }

        public Forgiveness(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);

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
                var amount = enemy.TotalHP/4;
                enemy.Restore(amount);
                (double mod, string mess) = enemy.ChangeAttStage(-2);
                Result.StatChangeMessages.Add(mess);

                Result.EnemyHeal = amount;
            }
            return Result;
        }
    }
}