using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class StaticShock : BasicMove
    {
        public override string Name { get; } = "Static Shock";
        public override string Description { get; } = "The user shocks the opponent, dealing damage. This has a chance to paralyze.";
        public override BasicType Type { get; } = new ElectricType(true);
        public override bool Contact { get; } = true;
        public override int Power { get; } = 60;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 25;
        public override string TargetType { get; } = "SingleEnemy";
        
        public StaticShock() :base()
        {

        }

        public StaticShock(bool newmove) :base(newmove)
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
                        Result[TargetNum].StatusMessages.Add(t.SetParalysis());
                }
            }
            
            return Result;
        }
    }
}