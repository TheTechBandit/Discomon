using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class Harden : BasicMove
    {
        public override string Name { get; } = "Harden";
        public override string Description { get; } = "The user seals any cracks in its defenses, raising it's defense by one stage.";
        public override BasicType Type { get; } = new RockType(true);
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = -1;
        public override int MaxPP { get; } = 20;
        public override string TargetType { get; } = "Self";
        
        public Harden() :base()
        {

        }

        public Harden(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            ResetResult();
            AddResult();

            //Fail logic
            if(SelfMoveFailLogic(owner))
            {
                Result[TargetNum].Fail = true;
                Result[TargetNum].Hit = false;
            }
            //Hit logic
            else
            {
                CurrentPP--;
                (double mod, string mess) = owner.ChangeDefStage(1);
                Result[TargetNum].StatChangeMessages.Add(mess);
            }
            
            return Result;
        }
    }
}