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
        public MoveResult Result { get; set; }

        public BasicMove()
        {

        }

        public BasicMove(bool newmove)
        {
            CurrentPP = MaxPP;
            Disabled = false;
        }

        public virtual MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            return Result;
        }

        public void ResetResult()
        {
            Result = new MoveResult();
            Result.Move = this;
        }

        public int ApplyPower(CombatInstance inst, BasicMon owner)
        {
            var enemy = inst.GetOtherMon(owner);
            var mod = CalculateMod(inst, owner, enemy);
            (double atkmod, string str) = owner.ChangeAttStage(0);
            (double defmod, string str2) = enemy.ChangeDefStage(0);
            double dmg = (((((2.0*owner.Level)/5.0)+2.0) * Power * (((double)owner.CurStats[1]*atkmod)/((double)enemy.CurStats[2]*defmod))/50)+2)*mod;
            int damage = (int)dmg;

            if(damage < 1)
                damage = 1;

            Result.EnemyDmg = damage;

            return damage;
        }

        public double CalculateMod(CombatInstance inst, BasicMon owner, BasicMon enemy)
        {
            var mod = ModCrit(inst, owner, enemy) * ModRandom() * ModType(enemy);
            Result.Mod = mod;
            Console.WriteLine($"Mod: {mod}");
            return mod;
        }

        public double ModCrit(CombatInstance instance, BasicMon owner, BasicMon enemy)
        {
            switch(owner.CritChance)
            {
                case 0:
                    if(RandomGen.PercentChance(6.25))
                    {
                        Result.Crit = true;
                        Result.ModCrit = 1.5;
                        return 1.5;
                    }
                    break;
                case 1:
                    if(RandomGen.PercentChance(12.5))
                    {
                        Result.Crit = true;
                        Result.ModCrit = 1.5;
                        return 1.5;
                    }
                    break;
                case 2:
                    if(RandomGen.PercentChance(50.0))
                    {
                        Result.Crit = true;
                        Result.ModCrit = 1.5;
                        return 1.5;
                    }
                    break;
                default:
                    Result.Crit = true;
                    Result.ModCrit = 1.5;
                    return 1.5;
            }

            return 1.0;
        }

        public double ModRandom()
        {
            var random = RandomGen.RandomDouble(0.85, 1.0);
            Result.ModRand = random;
            return random;
        }

        public double ModType(BasicMon enemy)
        {
            var type = Type.ParseEffectiveness(enemy.Typing);
            if(type > 1)
                Result.SuperEffective = true;
            if(type < 1)
                Result.NotEffective = true;
            if(type == 0)
                Result.Immune = true;
            Result.ModType = type;
            
            return type;
        }

        public void Restore()
        {
            CurrentPP = MaxPP;
            Disabled = false;
            Result = null;
        }
    }
}