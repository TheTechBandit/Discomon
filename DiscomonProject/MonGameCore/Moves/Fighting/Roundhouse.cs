using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Roundhouse : BasicMove
    {
        public override string Name { get; } = "Roundhouse";
        public override string Description { get; } = "The user spins around before kicking the target in the face, this intimidates them, sharply lowering their attack";
        public override BasicType Type { get; } = new FightingType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 70;
        public override int Accuracy { get; } = 70;
        public override int MaxPP { get; } = 15;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Roundhouse() :base()
        {

        }

        public Roundhouse(bool newmove) :base(newmove)
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
                    (double mod, string mess) = t.ChangeAttStage(-2);
                    Result[TargetNum].StatChangeMessages.Add(mess);
                }
            }
            
            return Result;
        }
    }
}