using System;
using System.Collections.Generic;

namespace DiscomonProject
{
    public class CombatCreationTool
    {
        public List<List<UserAccount>> Players { get; set; }
        //Single, Double, Freeforall, 
        public string CombatType { get; set; }
        //If false, all mon are assumed to be level 50
        public bool NaturalLevels { get; set; }
        public bool ItemsOn { get; set; }
        public int MonsPerTeam { get; set; }

        public CombatCreationTool()
        {
            
        }

        public CombatCreationTool(string combatType)
        {
            CombatType = combatType;
            NaturalLevels = true;
            ItemsOn = true;
            MonsPerTeam = 6;
        }
        
    }
}