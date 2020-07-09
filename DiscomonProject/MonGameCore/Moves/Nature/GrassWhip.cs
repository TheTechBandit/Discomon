using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class GrassWhip : BasicMove
    {
        public override string Name { get; } = "Grass Whip";
        public override string Description { get; } = "The user attacks its opponent with vines, dealing damage.";
        public override BasicType Type { get; } = new NatureType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 45;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 25;
        public override string TargetType { get; } = "SingleEnemy";
        
        public GrassWhip() :base()
        {

        }

        public GrassWhip(bool newmove) :base(newmove)
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