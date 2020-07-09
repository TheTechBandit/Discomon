using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class MeteorStrike : BasicMove
    {
        public override string Name { get; } = "Meteor Strike";
        public override string Description { get; } = "The user calls down a meteor, dealing massive damage.";
        public override BasicType Type { get; } = new FireType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 90;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 5;
        public override string TargetType { get; } = "SingleEnemy";
        
        public MeteorStrike() :base()
        {

        }

        public MeteorStrike(bool newmove) :base(newmove)
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
                }
            }
            
            return Result;
        }
    }
}