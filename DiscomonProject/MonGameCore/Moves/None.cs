using DiscomonProject.MonGameCore.Types;
using DiscomonProject.Users;

namespace DiscomonProject.MonGameCore.Moves
{
    public class None : BasicMove
    {
        public override string Name { get; } = "None";
        public override string Description { get; } = "";
        public override BasicType Type { get; } = new BeastType();
        public override int MaxPP { get; } = 0;
        public override int Power { get; } = 0;
        public override int Accuracy { get; } = 0;
        
        public None()
        {

        }

        public None(bool newmove) :base(newmove)
        {

        }
        
        public override void ApplyMove(Character owner)
        {
            
        }
    }
}