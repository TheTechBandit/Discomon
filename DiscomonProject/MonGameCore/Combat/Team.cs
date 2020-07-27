using System.Collections.Generic;

namespace DiscomonProject
{
    public class Team
    {
        public string TeamName { get; set; }
        public List<UserAccount> Members { get; set; }
        public int TeamNum { get; set; }
        public int MultiNum { get; set; }
        public bool Eliminated { get; set; }
        public int TeamR { get; set; }
        public int TeamG { get; set; }
        public int TeamB { get; set; }
        public string Picture { get; set; }
        //OwnerOnly - only the team owner can send invites
        //AllMembers - anyone can send invites
        //NoPerms - anyone can send invites or kick or edit settings
        public string InvitePerms { get; set; }
        public bool OpenInvite { get; set; }

        public Team()
        {

        }

        public Team(bool newteam)
        {
            TeamName = NameGenerator();
            Members = new List<UserAccount>();
            MultiNum = 1;
            Eliminated = false;
            TeamR = 62;
            TeamG = 255;
            TeamB = 62;
            Picture = "https://cdn.discordapp.com/emojis/732682490833141810.png?v=1";
            InvitePerms = "OwnerOnly";
            OpenInvite = false;
        }

        public void AddMember(UserAccount user)
        {
            Members.Add(user);
        }

        public UserAccount TeamLeader()
        {
            if(Members.Count != 0)
                return Members[0];
            return null;
        }

        public bool IsTeamLeader(UserAccount user)
        {
            if(Members.Count != 0)
            {
                if(user.UserId == Members[0].UserId)
                    return true;
                else
                    return false;
            }

            return false;
        }

        public bool CanInvite(UserAccount user)
        {
            if(IsTeamLeader(user) || InvitePerms.Contains("AllMembers") || InvitePerms.Contains("NoPerms"))
                return true;
            return false;
        }

        public bool CanKick(UserAccount user)
        {
            if(IsTeamLeader(user) || InvitePerms.Contains("NoPerms"))
                return true;
            return false;
        }

        public bool CanAccessSettings(UserAccount user)
        {
            if(IsTeamLeader(user) || InvitePerms.Contains("NoPerms"))
                return true;
            return false;
        }

        public bool CanDisband(UserAccount user)
        {
            if(IsTeamLeader(user))
                return true;
            return false;
        }

        public string NameGenerator()
        {
            List<string> adjs = new List<string> 
            {
                "Angry", "Snoozy", "Smug", "Sad", "Poofy", "Extraordinary", "Raging", "Ecstatic", "Amazing", "Strong", "Pouty", "Muddy", 
                "Drenched", "Flawless", "Tricky", "Pushy", "Greasy", "Elegant", "Scared", "Wonderful", "Hungry", "Brave", "Happy", "Clumsy", 
                "Overwhelming", "Smart", "Smelly", "Handsome", "Cute", "Sleepy", "Sweet", "Slippery", "Envious", "Wiggly", "Silent", "Sneaky"
            };

            List<string> nouns = new List<string> 
            {
                "Snorils", "Sukis", "Ooks", "Arness", "Elecutes", "Grasipups", "Meliosas", "Psygoats", "Sedimo", "Smoledge", "Stebbles", 
                "Trees", "Tomatos", "Mountains", "Leaves", "River", "Lake", "Stars", "Moons", "Wagons", "Apples", "Pineapples", "Swords",
                "Bucklers", "Bookshelves", "Lamps", "Grass", "Cactus"
            };

            string name = $"Team {adjs[RandomGen.RandomInt(0, adjs.Count-1)]} {nouns[RandomGen.RandomInt(0, nouns.Count-1)]}";

            return name;
        }

        public override string ToString()
        {
            string str = "";
            if(Members.Count == 1)
            {
                str += $"{Members[0].Name}";
            }
            else if(Members.Count == 2)
            {
                str += $"{Members[0].Name} and {Members[1].Name}";
            }
            else
            {
                for(int i = 0; i < Members.Count-1; i++)
                {
                    str += $"{Members[i].Name}, ";
                }
                str += $"and {Members[Members.Count-1].Name}";
            }

            return str;
        }
    }
}