using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Lightning : BasicMove
    {
        public override string Name { get; } = "Lightning";
        public override string Description { get; } = "The user strikes the opponent with a bolt of lightning, dealing damage.";
        public override BasicType Type { get; } = new ElectricType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 80;
        public override int Accuracy { get; } = 90;
        public override int MaxPP { get; } = 15;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Lightning() :base()
        {

        }

        public Lightning(bool newmove) :base(newmove)
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