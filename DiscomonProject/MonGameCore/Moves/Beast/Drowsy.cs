using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Drowsy : BasicMove
    {
        public override string Name { get; } = "Drowsy";
        public override string Description { get; } = "The user becomes drowsy and falls asleep, healing its HP to full and removing status conditions.";
        public override BasicType Type { get; } = new BeastType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 10;
        public override string TargetType { get; } = "Self";
        
        public Drowsy() :base()
        {

        }

        public Drowsy(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();
            AddResult();

            //Fail logic
            if(SelfMoveFailLogicIgnoreStatus(owner))
            {
                Console.WriteLine("DRFAILA");
                Result[TargetNum].Fail = true;
                Result[TargetNum].Hit = false;
            }
            //Hit logic
            else
            {
                Console.WriteLine("DRA");
                CurrentPP--;
                Console.WriteLine("DRB");
                owner.Status.StatusCure();
                Console.WriteLine("DRC");
                Result[TargetNum].StatusMessages.Add(owner.SetAsleep(2));
                Console.WriteLine("DRD");
                owner.CurrentHP = owner.TotalHP;
                Console.WriteLine("DRE");
            }
            
            return Result;
        }
    }
}