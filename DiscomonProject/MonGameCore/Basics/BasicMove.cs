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
        public bool Buffered { get; set; }

        public BasicMove()
        {

        }

        public BasicMove(bool newmove)
        {
            CurrentPP = MaxPP;
            Disabled = false;
            Buffered = false;
        }

        public virtual MoveResult ApplyMove(CombatInstance inst, BasicMon owner)
        {
            return Result;
        }

        public virtual MoveResult ApplyBufferedMove(CombatInstance inst, BasicMon owner)
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
            var power = Power;
            (double atkmod, string str) = owner.ChangeAttStage(0);
            (double defmod, string str2) = enemy.ChangeDefStage(0);

            if(Result.Crit && owner.GetAttStage() < 0)
                atkmod = 1.0;
            if(Result.Crit && enemy.GetDefStage() > 0)
                defmod = 1.0;

            if(Type.Type == "Electric" && owner.Status.Charged)
            {
                power *= 2;
                owner.Status.Charged = false;
            }
                
            double dmg = (((((2.0*owner.Level)/5.0)+2.0) * power * (((double)owner.CurStats[1]*atkmod)/((double)enemy.CurStats[2]*defmod))/50)+2)*mod;
            int damage = (int)dmg;

            if(damage < 1)
                damage = 1;

            Result.EnemyDmg = damage;

            return damage;
        }

        public int ApplyPowerAlwaysCrit(CombatInstance inst, BasicMon owner)
        {
            var enemy = inst.GetOtherMon(owner);
            var mod = CalculateModAlwaysCrit(inst, owner, enemy);
            var power = Power;
            (double atkmod, string str) = owner.ChangeAttStage(0);
            (double defmod, string str2) = enemy.ChangeDefStage(0);

            if(Result.Crit && owner.GetAttStage() < 0)
                atkmod = 1.0;
            if(Result.Crit && enemy.GetDefStage() > 0)
                defmod = 1.0;

            if(Type.Type == "Electric" && owner.Status.Charged)
            {
                power *= 2;
                owner.Status.Charged = false;
            }

            double dmg = (((((2.0*owner.Level)/5.0)+2.0) * power * (((double)owner.CurStats[1]*atkmod)/((double)enemy.CurStats[2]*defmod))/50)+2)*mod;
            int damage = (int)dmg;

            if(damage < 1)
                damage = 1;

            Result.EnemyDmg = damage;

            return damage;
        }

        ///<summary>
        ///Rolls to determine if the move hit or not- true = hit false = miss
        ///</summary>
        public bool ApplyAccuracy(CombatInstance inst, BasicMon owner)
        {
            if(Accuracy >= 0)
            {
                var enemy = inst.GetOtherMon(owner);
                var adjustedAccuracy = Accuracy * owner.StatModAccEva(enemy.GetEvaStage());
                bool result = RandomGen.PercentChance(adjustedAccuracy);
                Result.ChanceToHit = adjustedAccuracy;
                return result;
            }
            else
            {
                return true;
            }
        }

        ///<summary>
        ///Calculates the total damage modifier
        ///<para>CritMod * RandomMod * TypeMod</para>
        ///</summary>
        public double CalculateMod(CombatInstance inst, BasicMon owner, BasicMon enemy)
        {
            var mod = ModCrit(inst, owner) * ModRandom() * ModType(enemy);
            Result.Mod = mod;
            Console.WriteLine($"Mod: {mod}");
            return mod;
        }

        public double CalculateModAlwaysCrit(CombatInstance inst, BasicMon owner, BasicMon enemy)
        {
            var mod = 1.5 * ModRandom() * ModType(enemy);
            Result.Crit = true;
            Result.ModCrit = 1.5;
            Result.Mod = mod;
            Console.WriteLine($"Mod: {mod}");
            return mod;
        }

        ///<summary>
        ///Rolls for a critical hit based on crit chance. Returns 1.5 if a crit lands.
        ///</summary>
        public double ModCrit(CombatInstance instance, BasicMon owner)
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
        
        ///<summary>
        ///Rolls for a random modifier between 0.85 and 1.0
        ///</summary>
        public double ModRandom()
        {
            var random = RandomGen.RandomDouble(0.85, 1.0);
            Result.ModRand = random;
            return random;
        }

        ///<summary>
        ///Determines the effectiveness of this move against an enemy based on typing.
        ///</summary>
        public double ModType(BasicMon enemy)
        {
            double type = 1.0;
            if(enemy.OverrideType)
                type = Type.ParseEffectiveness(enemy.OverrideTyping);
            else
                type = Type.ParseEffectiveness(enemy.Typing);
            
            if(type > 1)
                Result.SuperEffective = true;
            if(type < 1)
                Result.NotEffective = true;
            if(type == 0)
                Result.Immune = true;
            Result.ModType = type;
            
            return type;
        }

        ///<summary>
        ///Sets PP to max, sets disabled to false, and clears the move result
        ///</summary>
        public void Restore()
        {
            CurrentPP = MaxPP;
            Disabled = false;
            Result = null;
        }

        public bool DefaultFailLogic(BasicMon enemy, BasicMon owner)
        {
            if(StatusFailCheck(owner) || enemy.Fainted || enemy == null || owner.Fainted || Disabled || enemy.Status.Flying)
            {
                if(Disabled)
                    Result.FailText = $"{Name} is disabled!";
                if(enemy.Status.Flying)
                    Result.FailText = $"{enemy.Nickname} is flying too high to reach!";
                Buffered = false;
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DefaultFailLogic(BasicMon enemy, BasicMon owner, bool ignoreFly, bool ignoreUnderground, bool ignoreDive)
        {
            if(StatusFailCheck(owner) || enemy.Fainted || enemy == null || owner.Fainted || Disabled || (!ignoreFly && enemy.Status.Flying) /*|| (!ignoreUnderground && enemy.Status.Digging) || (!ignoreDive && enemy.Status.Diving)*/)
            {
                if(Disabled)
                    Result.FailText = $"{Name} is disabled!";
                Buffered = false;
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SelfMoveFailLogic(BasicMon owner)
        {
            if(StatusFailCheck(owner) || owner.Fainted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool StatusFailLogic(BasicMon enemy, string type)
        {
            if(enemy.TypingToString().Contains(type) || enemy.Status.Burned || enemy.Status.Paraylzed || enemy.Status.Poisoned || enemy.Status.BadlyPoisoned || enemy.Status.Frozen || enemy.Status.Asleep)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool StatusFailCheck(BasicMon owner)
        {
            if(owner.Status.Flinching == true)
            {
                Result.FailText = $"{owner.Nickname} flinched!";
                owner.Status.Flinching = false;
                return true;
            }
            if(owner.Status.Paraylzed == true)
            {
                if(RandomGen.PercentChance(25.0))
                {
                    Result.FailText = $"{owner.Nickname} is paralyzed!";
                    return true;
                }
            }
            if(owner.Status.Asleep == true)
            {
                Result.FailText = $"{owner.Nickname} is asleep!";
                owner.Status.SleepTick();
                return true;
            }
            if(owner.Status.Frozen == true)
            {
                if(owner.Status.FreezeTick())
                {
                    Result.FailText = $"{owner.Nickname} has unfrozen!";
                    return false;
                }
                else
                {
                    Result.FailText = $"{owner.Nickname} is frozen!";
                    return true;
                }
            }

            return false;
        }
    }
}