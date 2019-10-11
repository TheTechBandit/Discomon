using DiscomonProject.Users;

namespace DiscomonProject.MonGameCore
{
    public class BasicMove
    {
        public virtual string Name { get; }
        public virtual string Description { get; }
        public virtual BasicType Type { get; }
        public virtual int MaxPP { get; }
        public int CurrentPP { get; }
        public virtual int Power { get; }
        public virtual int Accuracy { get; }

        public BasicMove()
        {

        }

        public BasicMove(bool newmove)
        {
            CurrentPP = MaxPP;
        }
        
        public virtual void ApplyMove(Character owner)
        {

        }

        public int ApplyPower(CombatInstance inst)
        {
            var active = inst.ActiveMon;
            var enemy = inst.EnemyMon;

            return ((((((2*active.Level)/5)+2)*Power*(active.CurStats[1]/enemy.CurStats[2]))/50)+2);//*mod;
        }
    }
}