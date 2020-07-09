using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Crave : BasicMove
    {
        public override string Name { get; } = "Crave";
        public override string Description { get; } = "The opponent is hit with a sudden craving for a snack! This lowers the opponenet's defense.";
        public override BasicType Type { get; } = new FeyType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 40;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 25;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Crave() :base()
        {

        }

        public Crave(bool newmove) :base(newmove)
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
                    (double mod, string mess) = t.ChangeDefStage(-1);
                    Result[TargetNum].StatChangeMessages.Add(mess);
                }
            }
            
            return Result;
        }
    }
}