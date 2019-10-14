namespace DiscomonProject
{
    public class Tackle : BasicMove
    {
        public override string Name { get; } = "Tackle";
        public override string Description { get; } = "The user tackles their enemy, dealing damage.";
        public override BasicType Type { get; } = new BeastType();
        public override bool Contact { get; } = true;
        public override int MaxPP { get; } = 35;
        public override int Power { get; } = 40;
        public override int Accuracy { get; } = 100;
        
        public Tackle() :base()
        {

        }

        public Tackle(bool newmove) :base(newmove)
        {
            CurrentPP = MaxPP;
        }

        public override int ApplyMove(NewCombatInstance inst, BasicMon owner)
        {
            var enemy = inst.GetOtherMon(owner);
            int dmg = 0;

            //Fail logic
            if(enemy.Fainted || enemy == null || owner.Fainted /*|| enemy.Flying*/)
            {
                owner.MoveFailed = true;
            }
            //Hit logic
            else
            {
            dmg = ApplyPower(inst, owner);
            enemy.TakeDamage(dmg);
            enemy.DmgHit = true;
            }
            return dmg;
        }
    }
}