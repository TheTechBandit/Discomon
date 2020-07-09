using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class IronCharge : BasicMove
    {
        public override string Name { get; } = "Iron Charge";
        public override string Description { get; } = "The user charges the enemy blindly. This move will lower the defense of the user.";
        public override BasicType Type { get; } = new MetalType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 100;
        public override int Accuracy { get; } = 90;
        public override int MaxPP { get; } = 5;
        public override string TargetType { get; } = "SingleEnemy";
        
        public IronCharge() :base()
        {

        }

        public IronCharge(bool newmove) :base(newmove)
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
                    (double mod, string mess) = owner.ChangeDefStage(-1);
                    Result[TargetNum].StatChangeMessages.Add(mess);
                }
            }
            
            return Result;
        }
    }
}