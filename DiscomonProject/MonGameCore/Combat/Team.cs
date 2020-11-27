using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class Team
    {
        public string TeamName { get; set; }
        public List<ulong> MemberIDs { get; set; }
        //Members should not be stored. This causes problems with the Team Member User Objects not having the same Object ID as the UserHandler User Objects.
        [JsonIgnore]
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
        public string Permissions { get; set; }
        public bool OpenInvite { get; set; }

        public Team()
        {
            
        }

        public Team(bool newteam)
        {
            TeamName = NameGenerator();
            MemberIDs = new List<ulong>();
            Members = new List<UserAccount>();
            MultiNum = 1;
            Eliminated = false;
            TeamR = RandomGen.RandomInt(0, 255);
            TeamG = RandomGen.RandomInt(0, 255);
            TeamB = RandomGen.RandomInt(0, 255);
            Picture = "https://cdn.discordapp.com/emojis/732682490833141810.png?v=1";
            Permissions = "OwnerOnly";
            OpenInvite = false;
        }

        public void AddMember(UserAccount user)
        {
            MemberIDs.Add(user.UserId);
            Members.Add(user);
        }

        public void KickMember(UserAccount user)
        {
            Console.WriteLine($"Z {Members.Count}\n{ToString()}");
            if(Members.Contains(user))
            {
                Console.WriteLine($"A {Members.Count}");
                MemberIDs.Remove(user.UserId);
                Members.Remove(user);
                Console.WriteLine($"B {Members.Count}");
            }
            Console.WriteLine("C");
        }

        /*
        FIXES THE OBJECT IDS OF THE TEAM VARIABLES
        This needs to be done because when the bot shuts down and boots back up, user objects that are saved in the team
        become desynchronized from the user objects saved in the UserHandler.
        */
        public void LoadUsers()
        {
            Members = new List<UserAccount>();

            for(int i = 0; i < MemberIDs.Count; i++)
            {
                Members.Add(UserHandler.GetUser(MemberIDs[i]));
            }
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
            if(IsTeamLeader(user) || Permissions.Contains("AllMembers") || Permissions.Contains("NoPerms"))
                return true;
            return false;
        }

        public bool CanKick(UserAccount user)
        {
            if(IsTeamLeader(user) || Permissions.Contains("NoPerms"))
                return true;
            return false;
        }

        public bool CanAccessSettings(UserAccount user)
        {
            if(IsTeamLeader(user) || Permissions.Contains("NoPerms"))
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
                "Overwhelming", "Smart", "Smelly", "Handsome", "Cute", "Sleepy", "Sweet", "Slippery", "Envious", "Wiggly", "Silent", "Sneaky",
                "Spicy", "Colossal", "Weary", "Clever", "Wandering", "Dry", "Fluffy", "Midnight", "Sparkling", "Temporary", "Fearless", "League of"
            };
            //Console.WriteLine($"Adjectives: {adjs.Count}");
            List<string> nouns = new List<string> 
            {
                "Snorils", "Sukis", "Ooks", "Arness", "Elecutes", "Grasipups", "Meliosas", "Psygoats", "Sedimo", "Smoledge", "Stebbles", 
                "Trees", "Tomatos", "Mountains", "Leaves", "River", "Lakes", "Stars", "Moons", "Wagons", "Apples", "Pineapples", "Swords",
                "Bucklers", "Bookshelves", "Lamps", "Grass", "Cactus", "Code", "Gamers", "Pirates", "Dreamers", "Society", "Fellowship",
                "Cheese", "Garbage", "Poets", "Soldiers", "Kings", "Queens", "Vagabonds", "Cookies", "Receptionists", "Secretaries"
            };
            //Console.WriteLine($"Nouns: {nouns.Count}");

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