using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Forge : BasicMove
    {
        public override string Name { get; } = "Forge";
        public override string Description { get; } = "The user covers itself in molten metal, curing itself of any status conditions and increasing its defense.";
        public override BasicType Type { get; } = new MetalType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 5;
        public override string TargetType { get; } = "Self";
        
        public Forge() :base()
        {

        }

        public Forge(bool newmove) :base(newmove)
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
                Result[TargetNum].Fail = true;
                Result[TargetNum].Hit = false;
            }
            //Hit logic
            else
            {
                CurrentPP--;
                owner.Status.StatusCure();

                (double mod, string mess) = owner.ChangeDefStage(2);
                Result[TargetNum].StatChangeMessages.Add(mess);
            }
            
            return Result;
        }
    }
}