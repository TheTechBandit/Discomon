using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Character
    {
        public string Name { get; set; }
        public string Mention { get; set; }
        public ulong CurrentGuildId { get; set; }
        public string CurrentGuildName { get; set; }
        public BasicMon ActiveMon { get; set; }
        public List<BasicMon> Party { get; set; }
        public List<BasicMon> PC { get; set; }
        public ulong CombatRequest { get; set; }
        public ulong InCombatWith { get; set; }
        public bool InCombat { get; set; }
        public bool InPvpCombat { get; set; }
        public int CombatId { get; set; }

        public Character()
        {
            
        }

        public Character(bool newchar)
        {
            Party = new List<BasicMon>();
            PC = new List<BasicMon>();
            InCombat = false;
            InPvpCombat = false;
            CombatId = -1;
        }

        public void ExitCombat()
        {
            InCombat = false;
            InPvpCombat = false;
            CombatRequest = 0;
            InCombatWith = 0;
            CombatId = -1;
            foreach(BasicMon mon in Party)
                mon.SelectedMove = null;
            ActiveMon = null;
        }

        public BasicMon FirstUsableMon()
        {
            for(int i = 0; i < Party.Count; i++)
            {
                if(Party[i].CurrentHP > 0)
                {
                    return Party[i];
                }
            }
            return null;
        }

        public bool HasLivingParty()
        {
            var dead = 0;
            foreach(BasicMon mon in Party)
            {
                if(mon.Fainted)
                {
                    dead++;
                }
            }
            if(dead == Party.Count || Party.Count < 1 || Party.Count > 6)
            {
                return false;
            }

            return true;
        }
    }
}