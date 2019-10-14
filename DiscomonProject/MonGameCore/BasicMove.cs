namespace DiscomonProject
{
    public class BasicMove
    {
        public virtual string Name { get; }
        public virtual string Description { get; }
        public virtual BasicType Type { get; }
        public virtual bool Contact { get; }
        public virtual int MaxPP { get; }
        public int CurrentPP { get; set; }
        public virtual int Power { get; }
        public virtual int Accuracy { get; }

        public BasicMove()
        {

        }

        public BasicMove(bool newmove)
        {
            CurrentPP = MaxPP;
        }

        public virtual int ApplyMove(CombatInstance inst, BasicMon owner)
        {
            return 0;
        }

        public int ApplyPower(CombatInstance inst, BasicMon owner)
        {
            var enemy = inst.GetOtherMon(owner);

            return ((((((2*owner.Level)/5)+2)*Power*(owner.CurStats[1]/enemy.CurStats[2]))/50)+2);//*mod;
        }
    }
}