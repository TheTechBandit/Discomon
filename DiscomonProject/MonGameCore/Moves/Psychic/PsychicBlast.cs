using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class PsychicBlast : BasicMove
    {
        public override string Name { get; } = "Psychic Blast";
        public override string Description { get; } = "The user unleashes a large psychic attack against the target, dealing damage.";
        public override BasicType Type { get; } = new PsychicType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 90;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 10;
        public override string TargetType { get; } = "AllEnemies";
        
        public PsychicBlast() :base()
        {

        }

        public PsychicBlast(bool newmove) :base(newmove)
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