using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Gear : BasicMove
    {
        public override string Name { get; } = "Gear";
        public override string Description { get; } = "The user adjusts the target, lowering their defense while bolstering their attack.";
        public override BasicType Type { get; } = new MetalType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 20;
        public override string TargetType { get; } = "SingleAll";
        
        public Gear() :base()
        {

        }

        public Gear(bool newmove) :base(newmove)
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
                if(DefaultFailLogic(t, owner))
                {
                    Result[TargetNum].Fail = true;
                    Result[TargetNum].Hit = false;
                }
                //Hit logic
                else
                {
                    CurrentPP--;
                    (double mod, string mess) = t.ChangeDefStage(-2);
                    Result[TargetNum].StatChangeMessages.Add(mess);

                    (double mod1, string mess1) = t.ChangeAttStage(2);
                    Result[TargetNum].StatChangeMessages.Add(mess1);
                }
            }
            
            return Result;
        }
    }
}