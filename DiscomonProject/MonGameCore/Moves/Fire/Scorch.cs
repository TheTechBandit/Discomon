using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Scorch : BasicMove
    {
        public override string Name { get; } = "Scorch";
        public override string Description { get; } = "The user scorches the target, burning them.";
        public override BasicType Type { get; } = new FireType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 15;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Scorch() :base()
        {

        }

        public Scorch(bool newmove) :base(newmove)
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
                if(DefaultFailLogic(t, owner) || StatusFailLogic(t, "Fire"))
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
                    Result[TargetNum].StatusMessages.Add(t.SetBurned());
                }
            }
            
            return Result;
        }
    }
}