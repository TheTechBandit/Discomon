using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Character
    {
        public string Name { get; set; }
        public ulong CurrentGuildId { get; set; }
        public string CurrentGuildName { get; set; }
        public List<BasicMon> Party { get; set; }
        public List<BasicMon> PC { get; set; }
        public ulong CombatRequest { get; set; }
        public ulong InCombatWith { get; set; }
        public bool InCombat { get; set; }
        public bool InPvpCombat { get; set; }
        public CombatInstance Combat { get; set; }

        public Character()
        {
            
        }

        public Character(bool newchar)
        {
            Party = new List<BasicMon>();
            PC = new List<BasicMon>();
            InCombat = false;
            InPvpCombat = false;
            Combat = null;
        }

        public void ExitCombat()
        {
            InCombat = false;
            InPvpCombat = false;
            CombatRequest = 0;
            InCombatWith = 0;
            Combat = null;
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
    }
}