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

        ///<summary>
        ///Rolls to determine if the move hit or not- true = hit false = miss
        ///</summary>
        public bool ApplyAccuracy(CombatInstance inst, BasicMon owner)
        {
            var enemy = inst.GetOtherMon(owner);
            var adjustedAccuracy = Accuracy * owner.StatModAccEva(enemy.GetEvaStage());
            bool result = RandomGen.PercentChance(adjustedAccuracy);
            Result.ChanceToHit = adjustedAccuracy;
            return result;
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
            if(StatusFailCheck(owner) || enemy.Fainted || enemy == null || owner.Fainted /*|| enemy.Flying*/)
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
            if(owner.Status.Paraylzed == true)
            {
                if(RandomGen.PercentChance(25.0))
                {
                    Result.FailText = "But it was paralyzed!";
                    return true;
                }
            }

            return false;
        }
    }
}