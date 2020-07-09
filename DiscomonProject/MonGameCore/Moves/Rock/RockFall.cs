using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class RockFall : BasicMove
    {
        public override string Name { get; } = "Rock Fall";
        public override string Description { get; } = "The user causes rocks to fall on top of their enemies. Has to chance to lower their speed.";
        public override BasicType Type { get; } = new RockType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 70;
        public override int Accuracy { get; } = 90;
        public override int MaxPP { get; } = 20;
        public override string TargetType { get; } = "AllEnemies";
        
        public RockFall() :base()
        {

        }

        public RockFall(bool newmove) :base(newmove)
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
                    if(RandomGen.PercentChance(20.0))
                    {
                        (double mod, string mess) = t.ChangeSpdStage(-1);
                        Result[TargetNum].StatChangeMessages.Add(mess);
                    }
                }
            }
            
            return Result;
        }
    }
}