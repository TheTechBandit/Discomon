namespace DiscomonProject
{
    public class None : BasicMove
    {
        public override string Name { get; } = "None";
        public override string Description { get; } = "";
        public override BasicType Type { get; } = new BeastType();
        public override bool Contact { get; } = false;
        public override int MaxPP { get; } = 0;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 0;
        
        public None() :base()
        {

        }

        public None(bool newmove) :base(newmove)
        {

        }

        public override int ApplyMove(CombatInstance inst, BasicMon owner)
        {
            return 0;
        }
    }
}