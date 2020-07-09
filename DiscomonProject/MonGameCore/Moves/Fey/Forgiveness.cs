using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Forgiveness : BasicMove
    {
        public override string Name { get; } = "Forgiveness";
        public override string Description { get; } = "The user forgives the opponent, healing the opponent and sharply lowering their attack.";
        public override BasicType Type { get; } = new FeyType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 10;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Forgiveness() :base()
        {

        }

        public Forgiveness(bool newmove) :base(newmove)
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
                    var amount = t.TotalHP/4;
                    t.Restore(amount);
                    (double mod, string mess) = t.ChangeAttStage(-2);
                    Result[TargetNum].StatChangeMessages.Add(mess);

                    Result[TargetNum].EnemyHeal = amount;
                }
            }
            
            return Result;
        }
    }
}