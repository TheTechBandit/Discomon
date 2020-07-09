using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Drench : BasicMove
    {
        public override string Name { get; } = "Drench";
        public override string Description { get; } = "The user drenches the enemy mon, changing its type to water.";
        public override BasicType Type { get; } = new FeyType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 100;
        public override int MaxPP { get; } = 20;
        public override string TargetType { get; } = "SingleEnemy";
        
        public Drench() :base()
        {

        }

        public Drench(bool newmove) :base(newmove)
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
                if(DefaultFailLogic(t, owner) || t.HasType("Water"))
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
                    t.OverrideType = true;
                    t.OverrideTyping.Add(new WaterType(true));
                    Result[TargetNum].Messages.Add($"{t.Nickname} is now a **Water** type!");
                }
            }
            
            return Result;
        }
    }
}