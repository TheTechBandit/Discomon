using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Wish : BasicMove
    {
        public override string Name { get; } = "Wish";
        public override string Description { get; } = "The user heals slightly, cures any status conditions, and raises their own attack and affinity.";
        public override BasicType Type { get; } = new FeyType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 5;
        public override string TargetType { get; } = "SingleAlly";
        
        public Wish() :base()
        {

        }

        public Wish(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();
            
            foreach(BasicMon t in targets)
            {
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
                    t.Status.StatusCure();
                    var amount = t.TotalHP/10;
                    t.Restore(amount);
                    Result[TargetNum].SelfHeal = amount;

                    (double mod, string mess) = t.ChangeAttStage(1);
                    Result[TargetNum].StatChangeMessages.Add(mess);

                    (double mod1, string mess1) = t.ChangeAffStage(1);
                    Result[TargetNum].StatChangeMessages.Add(mess1);
                }
            }
            
            return Result;
        }
    }
}