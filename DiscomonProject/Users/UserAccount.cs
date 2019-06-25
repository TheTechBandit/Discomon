using System.Collections;

namespace DiscomonProject
{
    public class UserAccount
    {
        public string Name { get; set; }
        public ulong CurrentGuildId { get; set; }
        public ulong UserId { get; set; }
        public string AvatarUrl { get; set; }
        public BasicMon[] Party { get; set; }
        public ArrayList PC { get; set; }
        public ulong CombatRequest { get; set; }
        public bool InCombat { get; set; }

        public UserAccount()
        {
            Party = new BasicMon[6];
            PC = new ArrayList();
            InCombat = false;
        }
        
    }
}