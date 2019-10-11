using DiscomonProject.MonGameCore.Types;
using DiscomonProject.Users;

namespace DiscomonProject.MonGameCore.Moves
{
    public class Tackle : BasicMove
    {
        public override string Name { get; } = "Tackle";
        public override string Description { get; } = "The user tackles their enemy, dealing damage.";
        public override BasicType Type { get; } = new BeastType();
        public override int MaxPP { get; } = 35;
        public override int Power { get; } = 40;
        public override int Accuracy { get; } = 100;
        
        public Tackle()
        {

        }

        public Tackle(bool newmove) :base(newmove)
        {

        }
        
        public override void ApplyMove(Character owner) 
            => owner.Combat.EnemyMon.TakeDamage(ApplyPower(owner.Combat));
    }
}