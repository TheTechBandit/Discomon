using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Slice : BasicMove
    {
        public override string Name { get; } = "Slice";
        public override string Description { get; } = "The user slashes at the enemy, dealing damage.";
        public override BasicType Type { get; } = new MetalType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 40;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 35;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Slice() :base()
        {

        }

        public Slice(bool newmove) :base(newmove)
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
                //Hit Logic
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