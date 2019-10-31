using System;
using Newtonsoft.Json;

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
        public bool Disabled { get; set; }
        [JsonIgnore]
        MoveResult Result { get; set; }

        public BasicMove()
        {

        }

        public BasicMove(bool newmove)
        {
            CurrentPP = MaxPP;
            Disabled = false;
            Result.Move = this;
        }

        public virtual int ApplyMove(CombatInstance inst, BasicMon owner)
        {
            return 0;
        }

        public int ApplyPower(CombatInstance inst, BasicMon owner)
        {
            var enemy = inst.GetOtherMon(owner);
            var mod = CalculateMod(inst, owner, enemy);
            var damage = (int)(((((((2*owner.Level)/5)+2)*Power*(owner.CurStats[1]/enemy.CurStats[2]))/50)+2)*mod);

            if(damage < 1)
                damage = 1;

            return damage;
        }

        public double CalculateMod(CombatInstance inst, BasicMon owner, BasicMon enemy)
        {
            var mod = ModCrit(inst, owner, enemy) * ModRandom() * ModType(enemy);
            Console.WriteLine($"Mod: {mod}");
            return mod;
        }

        public double ModCrit(CombatInstance instance, BasicMon owner, BasicMon enemy)
        {
            switch(owner.CritChance)
            {
                case 0:
                    if(RandomGen.PercentChance(6.25))
                        return 1.5;
                    break;
                case 1:
                    if(RandomGen.PercentChance(12.5))
                        return 1.5;
                    break;
                case 2:
                    if(RandomGen.PercentChance(50.0))
                        return 1.5;
                    break;
                default:
                    return 1.5;
            }

            return 1.0;
        }

        public double ModRandom()
        {
            return RandomGen.RandomDouble(0.85, 1.0);
        }

        public double ModType(BasicMon enemy)
        {
            return Type.ParseEffectiveness(enemy.Typing);
        }

        public void Restore()
        {
            CurrentPP = MaxPP;
            Disabled = false;
        }
    }
}