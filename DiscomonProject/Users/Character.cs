using System.Collections.Generic;
using System.Linq;
using DiscomonProject.MonGameCore;

namespace DiscomonProject.Users
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
            => Party.FirstOrDefault(t => t.CurrentHP > 0);
    }
}