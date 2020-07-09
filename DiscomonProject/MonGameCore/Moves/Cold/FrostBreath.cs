using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class FrostBreath : BasicMove
    {
        public override string Name { get; } = "Frost Breath";
        public override string Description { get; } = "The user breathes out a torrent of freezing cold air towards the opponent, this move is always a critical hit.";
        public override BasicType Type { get; } = new ColdType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 60;
        public override int Accuracy { get; } = 95;
        public override int MaxPP { get; } = 10;
        public override string TargetType { get; } = "AllEnemies";
        
        public FrostBreath() :base()
        {

        }

        public FrostBreath(bool newmove) :base(newmove)
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
                    dmg = ApplyPowerAlwaysCrit(inst, owner, t);
                    t.TakeDamage(dmg);
                    if(RandomGen.PercentChance(10.0))
                        Result[TargetNum].StatusMessages.Add(t.SetFrozen());
                }
            }
            
            return Result;
        }
    }
}