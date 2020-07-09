using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Gust : BasicMove
    {
        public override string Name { get; } = "Gust";
        public override string Description { get; } = "The user beats its wings, sending a gust of air at the opponent, dealing damage.";
        public override BasicType Type { get; } = new AirType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 50;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 30;
        public override string TargetType { get; } = "AllEnemies";
        
        public Gust() :base()
        {

        }

        public Gust(bool newmove) :base(newmove)
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