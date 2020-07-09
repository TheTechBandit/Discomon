using System;
using System.Collections.Generic;
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
        public virtual string TargetType { get; }
        public bool Disabled { get; set; }
        [JsonIgnore]
        public List<MoveResult> Result { get; set; }
        public bool Buffered { get; set; }
        public List<BasicMon> Targets { get; set; }
        public List<BasicMon> ValidTargets { get; set; }
        public int TargetNum { get; set; }

        public BasicMove()
        {

        }

        public BasicMove(bool newmove)
        {
            CurrentPP = MaxPP;
            Disabled = false;
            Result = new List<MoveResult>();
            Buffered = false;
            Targets = new List<BasicMon>();
            ValidTargets = new List<BasicMon>();
            TargetNum = -1;
        }

        //public virtual List<MoveResult> ApplyMove(CombatInstance inst, BasicMon owner)
        //{
            //return Result;
        //}

        public virtual List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            return Result;
        }

        public virtual List<MoveResult> ApplyMove(CombatInstance2 inst, BasicMon owner)
        {
            return Result;
        }

        public virtual List<MoveResult> ApplyBufferedMove(CombatInstance2 inst, BasicMon owner, List<BasicMon> targets)
        {
            return Result;
        }

        public void ResetResult()
        {
            Result.Clear();
            TargetNum = -1;
        }

        public void AddResult()
        {
            Result.Add(new MoveResult());
            TargetNum++;
            Result[TargetNum].Move = this;
            Result[TargetNum].Target = Targets[TargetNum];
        }

        public int ApplyPower(CombatInstance2 inst, BasicMon owner, BasicMon target)
        {
            var mod = CalculateMod(inst, owner, target);
            var power = Power;
            (double atkmod, string str) = owner.ChangeAttStage(0);
            (double defmod, string str2) = target.ChangeDefStage(0);

            if(Result[TargetNum].Crit && owner.GetAttStage() < 0)
                atkmod = 1.0;
            if(Result[TargetNum].Crit && target.GetDefStage() > 0)
                defmod = 1.0;

            if(Type.Type == "Electric" && owner.Status.Charged)
            {
                power *= 2;
                owner.Status.Charged = false;
            }
                
            double dmg = (((((2.0*owner.Level)/5.0)+2.0) * power * (((double)owner.CurStats[1]*atkmod)/((double)target.CurStats[2]*defmod))/50)+2)*mod;
            int damage = (int)dmg;

            if(damage < 1)
                damage = 1;

            Result[TargetNum].EnemyDmg = damage;

            return damage;
        }

        public int ApplyPowerAlwaysCrit(CombatInstance2 inst, BasicMon owner, BasicMon target)
        {
            var mod = CalculateModAlwaysCrit(inst, owner, target);
            var power = Power;
            (double atkmod, string str) = owner.ChangeAttStage(0);
            (double defmod, string str2) = target.ChangeDefStage(0);

            if(Result[TargetNum].Crit && owner.GetAttStage() < 0)
                atkmod = 1.0;
            if(Result[TargetNum].Crit && target.GetDefStage() > 0)
                defmod = 1.0;

            if(Type.Type == "Electric" && owner.Status.Charged)
            {
                power *= 2;
                owner.Status.Charged = false;
            }

            double dmg = (((((2.0*owner.Level)/5.0)+2.0) * power * (((double)owner.CurStats[1]*atkmod)/((double)target.CurStats[2]*defmod))/50)+2)*mod;
            int damage = (int)dmg;

            if(damage < 1)
                damage = 1;

            Result[TargetNum].EnemyDmg = damage;

            return damage;
        }

        ///<summary>
        ///Rolls to determine if the move hit or not- true = hit false = miss
        ///</summary>
        public bool ApplyAccuracy(CombatInstance2 inst, BasicMon owner, BasicMon target)
        {
            if(Accuracy >= 0)
            {
                var adjustedAccuracy = Accuracy * owner.StatModAccEva(target.GetEvaStage());
                bool result = RandomGen.PercentChance(adjustedAccuracy);
                Result[TargetNum].ChanceToHit = adjustedAccuracy;
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
        public double CalculateMod(CombatInstance2 inst, BasicMon owner, BasicMon enemy)
        {
            var mod = ModCrit(inst, owner) * ModRandom() * ModType(enemy) * ModWeather(inst);
            
            Result[TargetNum].Mod = mod;
            Console.WriteLine($"Mod: {mod}");
            return mod;
        }

        public double CalculateModAlwaysCrit(CombatInstance2 inst, BasicMon owner, BasicMon enemy)
        {
            var mod = 1.5 * ModRandom() * ModType(enemy) * ModWeather(inst);

            Result[TargetNum].Crit = true;
            Result[TargetNum].ModCrit = 1.5;
            Result[TargetNum].Mod = mod;
            Console.WriteLine($"Mod: {mod}");
            return mod;
        }

        public double ModWeather(CombatInstance2 inst)
        {
            double mod = 1.0;
            if(Type.Type == "Nature" && inst.Environment.Sunrise)
            {
                mod = 1.5;
            }

            if(Type.Type == "Fire" && inst.Environment.Heatwave)
            {
                mod = 1.5;
            }

            if(Type.Type == "Water" && inst.Environment.Heatwave)
            {
                mod = 0.5;
            }

            if(Type.Type == "Shade" && inst.Environment.Moonrise)
            {
                mod = 1.5;
            }

            return mod;
        }

        ///<summary>
        ///Rolls for a critical hit based on crit chance. Returns 1.5 if a crit lands.
        ///</summary>
        public double ModCrit(CombatInstance2 instance, BasicMon owner)
        {
            switch(owner.CritChance)
            {
                case 0:
                    if(RandomGen.PercentChance(6.25))
                    {
                        Result[TargetNum].Crit = true;
                        Result[TargetNum].ModCrit = 1.5;
                        return 1.5;
                    }
                    break;
                case 1:
                    if(RandomGen.PercentChance(12.5))
                    {
                        Result[TargetNum].Crit = true;
                        Result[TargetNum].ModCrit = 1.5;
                        return 1.5;
                    }
                    break;
                case 2:
                    if(RandomGen.PercentChance(50.0))
                    {
                        Result[TargetNum].Crit = true;
                        Result[TargetNum].ModCrit = 1.5;
                        return 1.5;
                    }
                    break;
                default:
                    Result[TargetNum].Crit = true;
                    Result[TargetNum].ModCrit = 1.5;
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
            Result[TargetNum].ModRand = random;
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
                Result[TargetNum].SuperEffective = true;
            if(type < 1)
                Result[TargetNum].NotEffective = true;
            if(type == 0)
                Result[TargetNum].Immune = true;
            Result[TargetNum].ModType = type;
            
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
            Result[TargetNum].Owner = owner;
            if(StatusFailCheck(owner) || enemy.Fainted || enemy == null || owner.Fainted || Disabled || enemy.Status.Flying)
            {
                if(Disabled)
                    Result[TargetNum].FailText = $"{Name} is disabled!";
                if(enemy.Status.Flying)
                    Result[TargetNum].FailText = $"{enemy.Nickname} is flying too high to reach!";
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
            Result[TargetNum].Owner = owner;
            if(StatusFailCheck(owner) || enemy.Fainted || enemy == null || owner.Fainted || Disabled || (!ignoreFly && enemy.Status.Flying) /*|| (!ignoreUnderground && enemy.Status.Digging) || (!ignoreDive && enemy.Status.Diving)*/)
            {
                if(Disabled)
                    Result[TargetNum].FailText = $"{Name} is disabled!";
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
            Result[TargetNum].Owner = owner;
            if(StatusFailCheck(owner) || owner.Fainted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SelfMoveFailLogicIgnoreStatus(BasicMon owner)
        {
            Result[TargetNum].Owner = owner;
            if(VolatileStatusFailCheck(owner) || owner.Fainted)
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

        public bool VolatileStatusFailCheck(BasicMon owner)
        {
            if(owner.Status.Flinching == true)
            {
                Result[TargetNum].FailText = $"{owner.Nickname} flinched!";
                owner.Status.Flinching = false;
                return true;
            }

            return false;
        }

        public bool StatusFailCheck(BasicMon owner)
        {
            if(owner.Status.Flinching == true)
            {
                Result[TargetNum].FailText = $"{owner.Nickname} flinched!";
                owner.Status.Flinching = false;
                return true;
            }
            if(owner.Status.Paraylzed == true)
            {
                if(RandomGen.PercentChance(25.0))
                {
                    Result[TargetNum].FailText = $"{owner.Nickname} is paralyzed!";
                    return true;
                }
            }
            if(owner.Status.Asleep == true)
            {
                if(owner.Status.SleepTick())
                {
                    Result[TargetNum].Messages.Add($"{owner.Nickname} has woken up!");
                    return false;
                }
                else
                {
                    Result[TargetNum].FailText = $"{owner.Nickname} is asleep!";
                    return true;
                }
            }
            if(owner.Status.Frozen == true)
            {
                if(owner.Status.FreezeTick())
                {
                    Result[TargetNum].Messages.Add($"{owner.Nickname} has unfrozen!");
                    return false;
                }
                else
                {
                    Result[TargetNum].FailText = $"{owner.Nickname} is frozen!";
                    return true;
                }
            }

            return false;
        }

        public bool DoesMoveRequireTargets()
        {
            if(TargetType == "None" || TargetType == "Self")
                return false;
            else
                return true;
        }

        public bool DoesMoveRequireTargetingMenu(List<BasicMon> targets)
        {
            if(!DoesMoveRequireTargets() || TargetType == "AllEnemies" || TargetType == "AllAllies" || TargetType == "All" || targets.Count <= 1)
                return false;
            else if(TargetType.Contains("Single") && targets.Count > 1)
                return true;
            return true;
        }

        public List<BasicMon> DetermineValidTargets(CombatInstance2 inst, BasicMon owner)
        {
            List<BasicMon> targets = new List<BasicMon>();

            switch(TargetType)
            {
                case "None":
                    return targets;
                case "Self":
                    targets.Add(owner);
                    return targets;
                case "SingleEnemy":
                case "AllEnemies":
                    foreach(Team t in inst.Teams)
                    {
                        if(!t.Members.Contains(inst.GetPlayer(owner)))
                        {
                            foreach(UserAccount u in t.Members)
                            {
                                foreach(BasicMon mon in u.Char.ActiveMons)
                                {
                                    if(!mon.Fainted && mon != null)
                                        targets.Add(mon);
                                }
                            }
                        }
                    }
                    return targets;
                case "SingleAlly":
                case "AllAllies":
                    foreach(Team t in inst.Teams)
                    {
                        if(t.Members.Contains(inst.GetPlayer(owner)))
                        {
                            foreach(UserAccount u in t.Members)
                            {
                                foreach(BasicMon mon in u.Char.ActiveMons)
                                {
                                    if(!mon.Fainted && mon != null)
                                        targets.Add(mon);
                                }
                            }
                        }
                    }
                    return targets;
                case "All":
                case "SingleAll":
                    foreach(Team t in inst.Teams)
                    {
                        foreach(UserAccount u in t.Members)
                        {
                            foreach(BasicMon mon in u.Char.ActiveMons)
                            {
                                if(!mon.Fainted && mon != null && mon != owner)
                                    targets.Add(mon);
                            }
                        }
                    }
                    return targets;
                default:
                    Console.WriteLine($"The move {Name} has an unkown TargetType.");
                    break;
            }

            ValidTargets = targets;
            return targets;
        }

        public void WipeTargets()
        {
            ValidTargets.Clear();
            Targets.Clear();
        }

    }
}