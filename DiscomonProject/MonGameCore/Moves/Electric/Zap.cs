using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Zap : BasicMove
    {
        public override string Name { get; } = "Zap";
        public override string Description { get; } = "The user zaps the target, paralyzing them.";
        public override BasicType Type { get; } = new ElectricType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 15;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Zap() :base()
        {

        }

        public Zap(bool newmove) :base(newmove)
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
                if(DefaultFailLogic(t, owner) || StatusFailLogic(t, "Electric"))
                {
                    Result[TargetNum].Fail = true;
                    Result[TargetNum].Hit = false;
                }
                //Miss Logic
                else if(!ApplyAccuracy(inst, owner, t))
                {
                    Result[TargetNum].Miss = true;
                    Result[TargetNum].Hit = false;
                }
                //Hit logic
                else
                {
                    CurrentPP--;
                    Result[TargetNum].StatusMessages.Add(t.SetParalysis());
                }
            }
            
            return Result;
        }
    }
}