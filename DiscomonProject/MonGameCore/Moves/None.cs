using System.Collections.Generic;

namespace DiscomonProject
{
    public class None : BasicMove
    {
        public override string Name { get; } = "None";
        public override string Description { get; } = "";
        public override BasicType Type { get; } = new BeastType();
        public override bool Contact { get; } = false;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 0;
        public override int MaxPP { get; } = 0;
        
        public None() :base()
        {

        }

        public None(bool newmove) :base(newmove)
        {

        }

        public override List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner)
        {
            return Result;
        }
    }
}