using System.Collections;
using System.Collections.Generic;

namespace DiscomonProject
{
    public static class CombatHandler
    {
        //The key is the "from" request. Index 0 contains the User requesting, 2 is the guild ID and 3 is the channel ID
        public static Dictionary<ArrayList, UserAccount> CombatRequests;

        static CombatHandler()
        {
            CombatRequests = new Dictionary<ArrayList, UserAccount>();
        }

        public static void RequestedCombat(ArrayList from, UserAccount to)
        {
            
        }
    }
}