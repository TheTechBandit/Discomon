using System;

namespace DiscomonProject
{
    public class Meditate : BasicMove
    {
        public override string Name { get; } = "Meditate";
        public override string Description { get; } = "The user calms themselves, restoring 1/10 of their total HP and raising attack and defense.";
        public override BasicType Type { get; } = new FightingType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 15;
        
        public Meditate() :base()
        {

        }

        public Meditate(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            ResetResult();
            var enemy = inst.GetOtherMon(owner);

            //Fail logic
            if(SelfMoveFailLogic(owner))
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
                var amount = owner.TotalHP/10;
                owner.Restore(amount);
                Result.SelfHeal = amount;

                (double mod, string mess) = owner.ChangeAttStage(1);
                Result.StatChangeMessages.Add(mess);

                (double mod1, string mess1) = owner.ChangeDefStage(1);
                Result.StatChangeMessages.Add(mess1);
            }
            return Result;
        }
    }
}