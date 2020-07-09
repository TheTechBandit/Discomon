using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Headbutt : BasicMove
    {
        public override string Name { get; } = "Headbutt";
        public override string Description { get; } = "The user hits the opponent with its head, dealing damage and potentially causing them to flinch.";
        public override BasicType Type { get; } = new BeastType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 70;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 15;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Headbutt() :base()
        {

        }

        public Headbutt(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();
            AddResult();

            foreach(BasicMon t in targets)
            {
                int dmg = 0;

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
                    if(RandomGen.PercentChance(30.0))
                        t.SetFlinching();
                }
            }
            
            return Result;
        }
    }
}