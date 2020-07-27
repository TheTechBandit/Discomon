using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscomonProject.Discord;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class BasicMon
    {
        ///<summary>The species name of this mon EX: "Suki"</summary>
        public virtual string Species { get; }
        ///<summary>The art link for this mon EX: "https://cdn.discordapp.com/attachments/516760928423772163/601482394045775882/suki.png"</summary>
        public virtual string ArtURL { get; }
        ///<summary>The Base HP value for this species EX: 65</summary>
        public virtual int BaseHP { get; }
        ///<summary>The Base Attack value for this species EX: 65</summary>
        public virtual int BaseAtt { get; }
        ///<summary>The Base Defense value for this species EX: 65</summary>
        public virtual int BaseDef { get; }
        ///<summary>The Base Affinity value for this species EX: 65</summary>
        public virtual int BaseAff { get; }
        ///<summary>The Base Speed value for this species EX: 65</summary>
        public virtual int BaseSpd { get; }
        ///<summary>A list that represents the EVs another mon would gain by defeating this one EX: [0, 1, 0, 2, 0] would boost attack EVs by 1 and affinity EVs by 2</summary>
        [JsonIgnore]
        public virtual List<int> EvGains { get; }
        ///<summary>The Type(s) of this mon EX: [FireType, RockType]</summary>
        public virtual List<BasicType> Typing { get; set; }
        ///<summary>If OverrideType is true, this will be used instead of Typing. EX: [FireType, RockType]</summary>
        public List<BasicType> OverrideTyping { get; set; }
        ///<summary>If true, OverrideTyping is used instead of Typing. EX: true</summary>
        public bool OverrideType { get; set; }
        ///<summary>The dex number of this mon EX: 23</summary>
        public virtual int DexNum { get; }
        ///<summary>The dex entry of this mon EX: "Snoril is a very peaceful and calm mon"</summary>
        public virtual string DexEntry { get; }
        ///<summary>The nickname of this mon EX: "Tim"</summary>
        public string Nickname { get; set; }
        ///<summary>The gender of this mon</summary>
        public string Gender { get; set; }
        public ulong CatcherID { get; set; }
        public ulong OwnerID { get; set; }
        public int Level { get; set; }
        public List<int> BaseList;
        public List<int> Ivs;
        public List<int> Evs;
        public List<double> NatureMods;
        public List<int> CurStats;
        //Att, Def, Aff, Spd, Accuracy, Evasion
        public List<int> StatMods;
        public List<string> NatureList;
        public BasicMove SelectedMove;
        public BasicMove BufferedMove;
        public List<BasicMove> ActiveMoves;
        public List<MovesetItem> Moveset;
        public BasicAbility Ability;
        public string Nature { get; set; }
        public int TotalHP { get; set; }
        public int CurrentHP { get; set; }
        public int CritChance { get; set; }
        public bool Fainted { get; set; }
        public bool IsCombatActive { get; set; }
        public string GenderSymbol { get; set; }
        public StatusEffect Status { get; set; }
        public delegate Task CombatEvent(BasicMon owner, CombatInstance2 inst);
        public event CombatEvent EnteredCombat;

        public BasicMon()
        {

        }

        public BasicMon(bool deflt)
        {
            Status = new StatusEffect(true);
            Level = 40;
            Nickname = Species;
            Gender = RandomGender();
            CatcherID = 0;
            OwnerID = 0;
            InitializeLists();
            Ability = new NoneAbility(true, this);
            GenerateIvs();
            SetRandomNature();
            CritChance = 0;
            Heal();
        }

        public BasicMon(int customLvl, List<int> customIvs, List<int> customEvs, string customNature)
        {
            Status = new StatusEffect(true);
            Level = customLvl;
            Nickname = Species;
            Gender = RandomGender();
            CatcherID = 0;
            OwnerID = 0;
            InitializeLists();
            Ivs.Clear();
            Evs.Clear();
            Ivs.AddRange(customIvs);
            Evs.AddRange(customEvs);
            Nature = customNature;
            Ability = new NoneAbility(true, this);
            UpdateNatureMods();
            Heal();
        }

        private void InitializeLists()
        {
            BaseList = new List<int>();
            NatureList = new List<string>
            {
                "Rash/Att/Def", "Blunt/Att/Aff", "Careful/Att/Spd",
                "Peaceful/Def/Att", "Stable/Def/Aff", "Sturdy/Def/Spd",
                "Calm/Aff/Att", "Shy/Aff/Def", "Pensive/Aff/Spd",
                "Witty/Spd/Att", "Impulsive/Spd/Def", "Hasty/Spd/Aff"
            };
            Ivs = new List<int>();
            Evs = new List<int>();
            NatureMods =  new List<double>();
            CurStats = new List<int>();
            StatMods = new List<int>();
            ActiveMoves = new List<BasicMove>();
            Moveset = new List<MovesetItem>();
            Moveset.Add(new MovesetItem(1, new Tackle(true)));
            OverrideTyping = new List<BasicType>();
            OverrideType = false;

            for(int i = 0; i < 5; i++)
            {
                BaseList.Add(0);
                Ivs.Add(0);
                Evs.Add(0);
                NatureMods.Add(1.0);
                CurStats.Add(0);
                StatMods.Add(0);
            }
            //Evasion
            StatMods.Add(0);

            BaseList[0] = BaseHP;
            BaseList[1] = BaseAtt;
            BaseList[2] = BaseDef;
            BaseList[3] = BaseAff;
            BaseList[4] = BaseSpd;

            //Fill ActiveMoves with 4 blank moves
            for(int i = 0; i < 4; i++)
                ActiveMoves.Add(new None());
        }

        public virtual void MoveSetup()
        {

        }

        public void GenActiveMoves()
        {
            List<BasicMove> validMoves = new List<BasicMove>();

            foreach(MovesetItem move in Moveset)
            {
                if(move.LearnLevel <= Level)
                    validMoves.Add(move.Move);
            }

            validMoves.Reverse();
            
            for(int i = 0; i < 4; i++)
            {
                if(validMoves.Count >= 1)
                {
                    ActiveMoves[i] = validMoves[0];
                    validMoves.Remove(ActiveMoves[i]);
                }
            }
        }

        private void GenerateIvs()
        {
            for(int i = 0; i < 5; i++)
            {
                Ivs[i] = RandomGen.Gen.Next(1, 32);
            }
        }

        public void LevelUp()
        {
            UpdateLevel(Level+1);
        }

        public void UpdateLevel(int newLevel)
        {
            Level = newLevel;
            UpdateStats();
        }

        public void UpdateStats()
        {
            //HP Calculation
            TotalHP = (int)CurStats[0];
            CurStats[0] = (int)((double)(((2 * BaseHP + (int)Ivs[0] + (double)((int)Evs[0] / 4)) * Level) / 100) + Level + 10);
            
            CurrentHP = CurrentHP + ((int)CurStats[0] - TotalHP);
            TotalHP = (int)CurStats[0];

            //Att, Def, Aff, and Spd Calculation
            for(int i = 1; i < 5; i++)
            {
                CurStats[i] = (int)(double)(((2 * (int)BaseList[i] + (int)Ivs[i] + (double)((int)Evs[i] / 4)) * Level) / 100) + 5;
                CurStats[i] = (int)((int)CurStats[i] * (double)NatureMods[i]);
            }

            //Speed Paralysis adjustment
            if(Status.Paraylzed)
                CurStats[4] = (int)((int)CurStats[4] * 0.5);
        }

        public void AddEvs(ArrayList gainedEvs)
        {
            //If the total number of EVs is above the maximum of 510, exit this method before adding EVs
            if(GetTotalEvs() > 510)
            {
                return;
            }

            //Loop through all EVs
            for(int i = 0; i < 5; i++)
            {
                //Check that the current EV is at 255 or lower
                if((int)Evs[i] <= 255)
                {
                    //If adding the gained EV will result in over 255, set the current EV to 255 instead.
                    if(((int)Evs[i] + (int)gainedEvs[i]) > 255)
                    {
                        Evs[i] = 255;
                    }
                    else
                    {
                        Evs[i] = (int)Evs[i] + (int)gainedEvs[i];
                    }
                }
            }
        }

        public int GetTotalEvs()
        {
            int total = 0;
            for(int i = 0; i < 5; i++)
            {
                total += (int)Evs[i];
            }

            return total;
        }

        public void SetRandomNature()
        {
            string nat = (string)NatureList[RandomGen.Gen.Next(0, NatureList.Count)];
            string[] splitNat = nat.Split("/");
            Nature = splitNat[0];

            UpdateNatureMods();
        }

        public void UpdateNatureMods()
        {
            string fullNat = "";
            foreach(string str in NatureList)
            {
                if(str.Contains(Nature))
                {
                    fullNat = str;
                }
            }
            string[] splitNat = fullNat.Split("/");

            for(int i = 0; i < 5; i++)
            {
                NatureMods[i] = 1.0;
            }

            switch(splitNat[1])
            {
                case "Att":
                    NatureMods[1] = 1.1;
                    break;
                case "Def":
                    NatureMods[2] = 1.1;
                    break;
                case "Aff":
                    NatureMods[3] = 1.1;
                    break;
                case "Spd":
                    NatureMods[4] = 1.1;
                    break;
            }

            switch(splitNat[2])
            {
                case "Att":
                    NatureMods[1] = 0.9;
                    break;
                case "Def":
                    NatureMods[2] = 0.9;
                    break;
                case "Aff":
                    NatureMods[3] = 0.9;
                    break;
                case "Spd":
                    NatureMods[4] = 0.9;
                    break;
            }

            UpdateStats();
        }

        public string RandomGender()
        {
            if(RandomGen.Gen.Next(0, 2) == 1)
            {
                GenderSymbol = "♂";
                return "Male";
            }
            else
            {
                GenderSymbol = "♀";
                return "Female";
            }
        }

        public void Heal()
        {
            CurrentHP = TotalHP;
            ResetStatStages();
            Status.ResetAll();
            Fainted = false;
            UpdateStats();
        }

        public void Restore(int amt)
        {
            CurrentHP += amt;
            if(CurrentHP > TotalHP)
                CurrentHP = TotalHP;
        }

        //Note: -Attack- and +Affinity+ needs to be changed based on nature, not set in stone.
        public string CurStatsToString()
        {
            return $"```diff\nHP:{CurStats[0]}\n-Attack:{CurStats[1]}-\nDefense:{CurStats[2]}\n+Affinity:{CurStats[3]}+\nSpeed:{CurStats[4]}```";
        }

        public string BaseStatsToString()
        {
            return $"```diff\nHP:{BaseList[0]}\nAttack:{BaseList[1]}\nDefense:{BaseList[2]}\nAffinity:{BaseList[3]}\nSpeed:{BaseList[4]}```";
        }

        public string IvsToString()
        {
            string str = "```bash\n";

            str += CheckPerfectIv(0);
            str += $"HP:{Ivs[0]}\n";

            str += CheckPerfectIv(1);
            str += $"Attack:{Ivs[1]}\n";
            
            str += CheckPerfectIv(2);
            str += $"Defense:{Ivs[2]}\n";

            str += CheckPerfectIv(3);
            str += $"Affinity:{Ivs[3]}\n";

            str += CheckPerfectIv(4);
            str += $"Speed:{Ivs[4]}\n";

            str += "```";
            return str;
        }

        public string EvsToString()
        {
            string str = "```bash\n";

            str += CheckPerfectEv(0);
            str += $"HP:{Evs[0]}\n";

            str += CheckPerfectEv(1);
            str += $"Attack:{Evs[1]}\n";
            
            str += CheckPerfectEv(2);
            str += $"Defense:{Evs[2]}\n";

            str += CheckPerfectEv(3);
            str += $"Affinity:{Evs[3]}\n";

            str += CheckPerfectEv(4);
            str += $"Speed:{Evs[4]}\n";

            str += "```";
            return str;
        }

        //Helper method used in IvsToString()
        private string CheckPerfectIv(int index)
        {
            if(Ivs[index] == 31)
            {
                return "$";
            }
            else
            {
                return "";
            }
        }

        //Helper method used in EvsToString()
        private string CheckPerfectEv(int index)
        {
            if(Evs[index] == 255)
            {
                return "$";
            }
            else
            {
                return "";
            }
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if(CurrentHP < 0)
                CurrentHP = 0;
        }

        public void ResetStatStages()
        {
            for(int i = 0; i < StatMods.Count; i++)
            {
                StatMods[i] = 0;
            }
            UpdateStats();
        }

        //Determine the stat stage change string
        public string StatModString(string stat, int stage, int change)
        {
            string str = "";
            if(change > 0)
            {
                if(stage >= 6)
                {
                    str += $"{Nickname}'s {stat} won't go any higher!";
                }
                else if(change == 1)
                {
                    str += $"{Nickname}'s {stat} rose!";
                }
                else if(change == 2)
                {
                    str += $"{Nickname}'s {stat} sharply rose!";
                }
                else if(change >= 3)
                {
                    str += $"{Nickname}'s {stat} drastically rose!";
                }
            }
            else if(change < 0)
            {
                if(stage <= -6)
                {
                    str += $"{Nickname}'s {stat} won't go any lower!";
                }
                else if(change == -1)
                {
                    str += $"{Nickname}'s {stat} fell!";
                }
                else if(change == -2)
                {
                    str += $"{Nickname}'s {stat} sharply fell!";
                }
                else if(change <= -3)
                {
                    str += $"{Nickname}'s {stat} drastically fell!";
                }
            }

            return str;
        }

        //Calculate the stat modifier based on stat stages
        public double StatMod(int stage)
        {
            double mod = 0.0;
            double top = 2.0;
            double bottom = 2.0;
            if(stage > 0)
                top += stage;
            if(stage < 0)
                bottom += Math.Abs(stage);

            mod = top/bottom;
            return mod;
        }

        //Calculate the stat modifier FOR ACC/EVA based on stat stages
        public double StatModAccEva(int enemyEva)
        {
            var stage = GetAccStage() - enemyEva;
            double mod = 0.0;
            double top = 3.0;
            double bottom = 3.0;
            if(stage > 0)
                top += stage;
            if(stage < 0)
                bottom += Math.Abs(stage);

            mod = top/bottom;
            return mod;
        }

        //Modify a stat stage. Returns the modifier and the change string (if necessary). ATT, DEF, AFF, SPD, ACC, EVA
        public (double mod, string msg) ChangeStage(int stat, int amt)
        {
            int temp = StatMods[stat];
            double mod = 1.0;
            string str = "";
            string statStr = "";
            if(stat == 0)
                statStr = "attack";
            if(stat == 1)
                statStr = "defense";
            if(stat == 2)
                statStr = "affinity";
            if(stat == 3)
                statStr = "speed";
            if(stat == 4)
                statStr = "accuracy";
            if(stat == 5)
                statStr = "evasion";

            if(amt > 0 || amt < 0)
            {
                str = StatModString($"{statStr}", temp, amt);
            }

            temp += amt;
            if(temp < -6)
                temp = -6;
            if(temp > 6)
                temp = 6;

            if(stat >= 0 && stat < 4)
                mod = StatMod(temp);
            if(stat == 4 || stat == 5)
                mod = StatModAccEva(temp);

            StatMods[stat] = temp;

            UpdateStats();
            return (mod, str);
        }

        public (double mod, string msg) ChangeAttStage(int amt)
        {
            return ChangeStage(0, amt);
        }

        public int GetAttStage()
        {
            return StatMods[0];
        }

        public double GetAttMod()
        {
            return StatMod(GetAttStage());
        }

        public int GetAdjustedAtt()
        {
            return (int)(CurStats[1]*GetAttMod());
        }

        public (double mod, string msg) ChangeDefStage(int amt)
        {
            return ChangeStage(1, amt);
        }

        public int GetDefStage()
        {
            return StatMods[1];
        }

        public double GetDefMod()
        {
            return StatMod(GetDefStage());
        }

        public int GetAdjustedDef()
        {
            return (int)(CurStats[2]*GetDefMod());
        }

        public (double mod, string msg) ChangeAffStage(int amt)
        {
            return ChangeStage(2, amt);
        }

        public int GetAffStage()
        {
            return StatMods[2];
        }

        public double GetAffMod()
        {
            return StatMod(GetAffStage());
        }

        public int GetAdjustedAff()
        {
            return (int)(CurStats[3]*GetAffMod());
        }

        public (double mod, string msg) ChangeSpdStage(int amt)
        {
            return ChangeStage(3, amt);
        }

        public int GetSpdStage()
        {
            return StatMods[3];
        }

        public double GetSpdMod()
        {
            return StatMod(GetSpdStage());
        }

        public int GetAdjustedSpd()
        {
            return (int)(CurStats[4]*GetSpdMod());
        }

        public (double mod, string msg) ChangeAccStage(int amt)
        {
            return ChangeStage(4, amt);
        }

        public int GetAccStage()
        {
            return StatMods[4];
        }

        public double GetAccMod(int enemyEva)
        {
            return StatModAccEva(enemyEva);
        }

        public (double mod, string msg) ChangeEvaStage(int amt)
        {
            return ChangeStage(5, amt);
        }

        public int GetEvaStage()
        {
            return StatMods[5];
        }

        public string SetParalysis()
        {
            Status.Paraylzed = true;
            return $"{Nickname} has been afflicted with *Paralysis*";
        }

        public string SetBurned()
        {
            Status.Burned = true;
            return $"{Nickname} has been afflicted with *Burn*";
        }

        public string SetAsleep(int num)
        {
            Status.FallAsleep(num);
            return $"{Nickname} has been afflicted with *Sleep*";
        }

        public bool SleepyCheck()
        {
            if(Status.Sleepy >= 1)
            {
                Status.Sleepy++;
                if(Status.Sleepy >= 3)
                {
                    Status.Sleepy = 0;
                    SetAsleep(0);
                    return true;
                }
            }
            return false;
        }

        public string SetFrozen()
        {
            Status.Frozen = true;
            return $"{Nickname} has been afflicted with *Frozen*";
        }

        public void SetFlinching()
        {
            Status.Flinching = true;
        }

        public string StatusDamage()
        {
            if(Status.Burned)
            {
                TakeDamage((int)(CurrentHP*0.125));
                return "Burn";
            }
            return "None";
        }

        public string TypingToString()
        {
            string str = "";
            if(!OverrideType)
            {
                if(Typing.Count > 1)
                    str = $"{Typing[0].Type}/{Typing[1].Type}";
                else
                    str = $"{Typing[0].Type}";
                return str;
            }
            else
            {
                if(OverrideTyping.Count > 1)
                    str = $"{OverrideTyping[0].Type}/{OverrideTyping[1].Type}";
                else
                    str = $"{OverrideTyping[0].Type}";
                return str;
            }
        }

        public List<int> HPGradient()
        {
            double perc = HealthPercentage();
            perc *= 100;
            int g = 2*(int)perc;
            int r = 200-g;
            int b = 0;

            return new List<int>()
            {
                r, g, b,
            };
        }

        public double HealthPercentage()
        {
            return (double)CurrentHP/(double)TotalHP;
        }

        public bool HasDisabledMove()
        {
            foreach(BasicMove move in ActiveMoves)
            {
                if(move.Disabled)
                    return true;
            }
            return false;
        }

        ///<summary>Returns true if this mon has a typing with a name equal to the string parameter. Uses OverrideTyping if OverrideType is true.</summary>
        public bool HasType(string type)
        {
            if(OverrideType)
            {
                foreach(BasicType t in OverrideTyping)
                {
                    if(t.Type == type)
                        return true;
                }
            }
            else
            {
                foreach(BasicType t in Typing)
                {
                    if(t.Type == type)
                        return true;
                }
            }

            return false;
        }

        public void ExitCombat()
        {
            IsCombatActive = false;
            if(SelectedMove != null)
                SelectedMove.WipeTargets();
            SelectedMove = null;
            BufferedMove = null;
            Status.CombatReset();
            OverrideType = false;
            OverrideTyping.Clear();
            ResetStatStages();
        }

        public void OnEnteredCombat(CombatInstance2 inst)
        {
            EnteredCombat?.Invoke(this, inst);
        }
        
    }
}