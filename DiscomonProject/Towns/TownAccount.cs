using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscomonProject.Discord;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class TownAccount
    {
        public ulong GuildId { get; set; }
        public string Name { get; set; }
        public List<Team> Teams { get; set; }

        public TownAccount()
        {

        }
        public TownAccount(bool newtown)
        {
            Teams = new List<Team>();
        }

        public Team GetTeam(UserAccount user)
        {
            foreach(Team t in Teams)
            {
                foreach(UserAccount u in t.Members)
                {
                    if(u.UserId == user.UserId)
                        return t;
                }
            }

            return null;
        }
        
    }
}