using System;
using System.Collections.Generic;

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
        public override string TargetType { get; } = "Self";
        
        public Meditate() :base()
        {

        }

        public Meditate(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();
            AddResult();

            //Fail logic
            if(SelfMoveFailLogic(owner))
            {
                Result[TargetNum].Fail = true;
                Result[TargetNum].Hit = false;
            }
            //Hit logic
            else
            {
                CurrentPP--;
                var amount = owner.TotalHP/10;
                owner.Restore(amount);
                Result[TargetNum].SelfHeal = amount;

                (double mod, string mess) = owner.ChangeAttStage(1);
                Result[TargetNum].StatChangeMessages.Add(mess);

                (double mod1, string mess1) = owner.ChangeDefStage(1);
                Result[TargetNum].StatChangeMessages.Add(mess1);
            }
            
            return Result;
        }
    }
}