using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class BoulderSlam : BasicMove
    {
        public override string Name { get; } = "Boulder Slam";
        public override string Description { get; } = "The user slams the enemy with a boulder, dealing damage.";
        public override BasicType Type { get; } = new RockType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 90;
        public override int Accuracy { get; } = 80;
        public override int MaxPP { get; } = 10;
        public override string TargetType { get; } = "SingleEnemy";
        
        public BoulderSlam() :base()
        {

        }

        public BoulderSlam(bool newmove) :base(newmove)
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