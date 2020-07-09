using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Lullaby : BasicMove
    {
        public override string Name { get; } = "Lullaby";
        public override string Description { get; } = "The user sings a soothing melody, making the target fall asleep after one turn.";
        public override BasicType Type { get; } = new PsychicType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 10;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Lullaby() :base()
        {

        }

        public Lullaby(bool newmove) :base(newmove)
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
                if(DefaultFailLogic(t, owner) || t.Status.Asleep || t.Status.Sleepy >= 1)
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
                    t.Status.Sleepy = 1;
                    Result[TargetNum].Messages.Add($"{t.Nickname} is feeling sleepy...");
                }
            }
            
            return Result;
        }
    }
}