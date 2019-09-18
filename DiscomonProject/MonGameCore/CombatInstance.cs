using System.Collections;
using System.Collections.Generic;
using DiscomonProject.Discord;

namespace DiscomonProject
{
    public class CombatInstance
    {
        public ContextIds Location { get; set; }
        public ulong ThisPlayer { get; set; }
        public ulong OtherPlayer { get; set; }
        /*
        CombatPhase determines what step of combat is currently happening
        PHASES:
        -1- Combat starting (Mons sending out, activate abilities such as Intimidate)
        0- Pre turn, activate weather effects... "It is still raining!" or "The rain cleared up!"
        1- [INPUT] Awaiting both players to choose attacks
        2- [INPUT] Attack has been inputted. Awaiting other player.
        3- Pre-Attack phase (activate any abilities that trigger before attacks)
        4- Attacks register. Calculate whether it hit, damage, bonus effects of attacks
        5- Post-attack phase (activate any abilities that trigger after attacks such as Rough Skin)
        6- Death phase. Test if either mon died. If any did, faint them and send out the next mon if any (activate any abilities that trigger when an opponent faints such as Beast Boost)
        7- Return to phase 1

        notes: possibly remove 6, combine 3 & 4
         */
        public int CombatPhase { get; set; }
        public BasicMon ActiveMon { get; set; }
        public BasicMon EnemyMon { get; set; }

        public CombatInstance()
        {
            
        }

        public CombatInstance(ContextIds loc, ulong thisPlayer, ulong otherPlayer)
        {
            ThisPlayer = thisPlayer;
            OtherPlayer = otherPlayer;
            Location = loc;
            CombatPhase = -1;
        }

    }
}