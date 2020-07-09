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

        public Team()
        {

        }

        public Team(bool newteam)
        {
            TeamName = "Default Team Name";
            Members = new List<UserAccount>();
            MultiNum = 1;
            Eliminated = false;
        }

        public void AddMember(UserAccount user)
        {
            Members.Add(user);
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