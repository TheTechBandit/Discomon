using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class FlameSpin : BasicMove
    {
        public override string Name { get; } = "Flame Spin";
        public override string Description { get; } = "The user spins very quickly before rolling into the opponent, dealing damage and raising its own speed.";
        public override BasicType Type { get; } = new FireType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 50;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 25;
        public override string TargetType { get; } = "SingleEnemy";
        
        public FlameSpin() :base()
        {

        }

        public FlameSpin(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();
            
            foreach(BasicMon t in targets)
            {
                int dmg = 0;
                AddResult();

                //Fail logic
                if(DefaultFailLogic(t, owner))
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
                    dmg = ApplyPower(inst, owner, t);
                    t.TakeDamage(dmg);
                    (double mod, string mess) = owner.ChangeSpdStage(1);
                    Result[TargetNum].StatChangeMessages.Add(mess);
                }
            }
            
            return Result;
        }
    }
}