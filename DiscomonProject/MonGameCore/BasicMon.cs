using System;
using System.Collections;

namespace DiscomonProject
{
    public abstract class BasicMon
    {
        public abstract string Species { get; }
        public abstract string ArtURL { get; }
        public abstract int BaseHP { get; }
        public abstract int BaseAtt { get; }
        public abstract int BaseDef { get; }
        public abstract int BaseAff { get; }
        public abstract int BaseSpd { get; }
        public abstract ArrayList EvGains { get; }
        public abstract string Typing { get; set; }
        public abstract int DexNum { get; }
        public abstract string DexEntry { get; }
        public string Nickname { get; set; }
        public string Gender { get; set; }
        public UserAccount Catcher { get; set; }
        public UserAccount Owner { get; set; }
        public int Level { get; set; }
        public ArrayList BaseList;
        public ArrayList Ivs;
        public ArrayList Evs;
        public ArrayList NatureMods;
        public ArrayList CurStats;
        public readonly ArrayList NatureList = new ArrayList 
        {
            "Rash/Att/Def", "Blunt/Att/Aff", "Careful/Att/Spd",
            "Peaceful/Def/Att", "Stable/Def/Aff", "Sturdy/Def/Spd",
            "Calm/Aff/Att", "Shy/Aff/Def", "Pensive/Aff/Spd",
            "Witty/Spd/Att", "Impulsive/Spd/Def", "Hasty/Spd/Aff"
        };
        public string Nature { get; set; }
        public int TotalHP { get; set; }
        public int CurrentHP { get; set; }

        public BasicMon()
        {
            Level = 20;
            Nickname = Species;
            Gender = RandomGender();
            Catcher = null;
            Owner = null;
            InitializeLists();
            GenerateIvs();
            SetRandomNature();
            Heal();
        }

        public BasicMon(int customLvl, ArrayList customIvs, ArrayList customEvs, string customNature)
        {
            Level = customLvl;
            Nickname = Species;
            Gender = RandomGender();
            Catcher = null;
            Owner = null;
            InitializeLists();
            Ivs.Clear();
            Evs.Clear();
            Ivs.AddRange(customIvs);
            Evs.AddRange(customEvs);
            Nature = customNature;
            UpdateNatureMods();
            Heal();
        }

        private void InitializeLists()
        {
            BaseList = new ArrayList();
            Ivs = new ArrayList();
            Evs = new ArrayList();
            NatureMods =  new ArrayList();
            CurStats = new ArrayList();

            for(int i = 0; i < 6; i++)
            {
                BaseList.Add(0);
                Ivs.Add(0);
                Evs.Add(0);
                NatureMods.Add(1.0);
                CurStats.Add(0);
            }

            BaseList[0] = BaseHP;
            BaseList[1] = BaseAtt;
            BaseList[2] = BaseDef;
            BaseList[3] = BaseAff;
            BaseList[4] = BaseSpd;
        }

        private void GenerateIvs()
        {
            for(int i = 0; i < 6; i++)
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
        }

        public void AddEvs(ArrayList gainedEvs)
        {
            //If the total number of EVs is above the maximum of 510, exit this method before adding EVs
            if(GetTotalEvs() > 510)
            {
                return;
            }

            //Loop through all EVs
            for(int i = 0; i < 6; i++)
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
            for(int i = 0; i < 6; i++)
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

            for(int i = 0; i < 6; i++)
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
                return "Male";
            }
            else
            {
                return "Female";
            }
        }

        public void Heal()
        {
            CurrentHP = TotalHP;
        }

        public string CurStatsToString()
        {
            return $"```diff\nHP:{CurStats[0]}\n-Attack:{CurStats[1]}-\nDefense:{CurStats[2]}\n+Affinity:{CurStats[3]}+\nSpeed:{CurStats[4]}```";
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
            if((int)Ivs[index] == 31)
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
            if((int)Evs[index] == 255)
            {
                return "$";
            }
            else
            {
                return "";
            }
        }
        
    }
}