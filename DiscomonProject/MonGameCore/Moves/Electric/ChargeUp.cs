using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class ChargeUp : BasicMove
    {
        public override string Name { get; } = "Charge Up";
        public override string Description { get; } = "The user charges up, doubling the power of it's next electric type move.";
        public override BasicType Type { get; } = new ElectricType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 10;
        public override string TargetType { get; } = "Self";
        
        public ChargeUp() :base()
        {

        }

        public ChargeUp(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();
            AddResult();

            //Fail logic
            if(SelfMoveFailLogic(owner) || owner.Status.Charged)
            {
                Result[TargetNum].Fail = true;
                Result[TargetNum].Hit = false;
            }
            //Hit logic
            else
            {
                CurrentPP--;
                owner.Status.Charged = true;
                Result[TargetNum].Messages.Add($"{owner.Nickname} charges up!");
            }
            
            return Result;
        }
    }
}