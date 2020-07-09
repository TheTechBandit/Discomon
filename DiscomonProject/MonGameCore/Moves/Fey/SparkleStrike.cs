using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class SparkleStrike : BasicMove
    {
        public override string Name { get; } = "Sparkle Strike";
        public override string Description { get; } = "The user conjurs a sparkling comet which smashes into the enemy.";
        public override BasicType Type { get; } = new FeyType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 90;
        public override int Accuracy { get; } = 90;
        public override int MaxPP { get; } = 5;
        public override string TargetType { get; } = "SingleEnemy";
        
        public SparkleStrike() :base()
        {

        }

        public SparkleStrike(bool newmove) :base(newmove)
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