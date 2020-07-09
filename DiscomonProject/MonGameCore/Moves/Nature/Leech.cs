using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Leech : BasicMove
    {
        public override string Name { get; } = "Leech";
        public override string Description { get; } = "The user drains a small amount of HP from the opposing mon, damaging the opponent and healing itself.";
        public override BasicType Type { get; } = new NatureType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 90;
        public override int MaxPP { get; } = 10;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Leech() :base()
        {

        }

        public Leech(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();
            
            foreach(BasicMon t in targets)
            {
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
                    var amount = t.TotalHP/6;
                    owner.Restore(amount);
                    t.TakeDamage(amount);

                    Result[TargetNum].SelfHeal = amount;
                    Result[TargetNum].EnemyDmg = amount;
                }
            }
            
            return Result;
        }
    }
}