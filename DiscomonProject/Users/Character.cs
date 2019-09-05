using System.Collections;

namespace DiscomonProject
{
    public class Character
    {
        public string Name { get; set; }
        public ulong CurrentGuildId { get; set; }
        public string CurrentGuildName { get; set; }
        public ArrayList Party { get; set; }
        public ArrayList PC { get; set; }
        public ulong CombatRequest { get; set; }
        public ulong InCombatWith { get; set; }
        public bool InCombat { get; set; }
        public bool InPvpCombat { get; set; }

        public Character()
        {
            Party = new ArrayList();
            PC = new ArrayList();
            InCombat = false;
            InPvpCombat = false;
        }
    }
}