using System.Collections;
using System.Collections.Generic;
using DiscomonProject.Discord;

namespace DiscomonProject
{
    public class CombatInstance
    {
        public ContextIds Location { get; set; }
        public UserAccount PlayerOne { get; set; }
        public UserAccount PlayerTwo { get; set; }
        public Environment Environment { get; set; }
        /*
        CombatPhase determines what step of combat is currently happening
        PHASES:
        -1- Combat starting (Mons sending out)
        0- Mon(s) sent out, (activate abilities such as Intimidate)
        1- Pre turn, activate weather effects... "It is still raining!" or "The rain cleared up!"
        2- [INPUT] Awaiting both players to choose attacks
        3- Pre-Attack phase (activate any abilities that trigger before attacks)
        4- Attacks register. Calculate whether it hit, damage, bonus effects of attacks
        5- Post-hit on MonOne/MonTwo (activate rough skin, check for death. If dead, go to phase 6)
        6- Death phase. Test if either mon died. If any did, faint them and send out the next mon if any (activate any abilities that trigger when an opponent faints such as Beast Boost)
        7- Return to phase 1

        notes: possibly remove 6, combine 3 & 4
         */
        public int CombatPhase { get; set; }
        public bool IsPvP { get; set; }
        public bool IsDoubleBattle { get; set; }

        public CombatInstance()
        {
            
        }

        public CombatInstance(ContextIds loc, UserAccount one, UserAccount two)
        {
            Location = loc;
            PlayerOne = one;
            PlayerTwo = two;
            Environment = new Environment(true);
            IsPvP = true;
            IsDoubleBattle = false;
            CombatPhase = -1;

            PlayerOne.Char.InCombat = true;
            PlayerOne.Char.InPvpCombat = true;
            PlayerOne.Char.CombatRequest = 0;
            PlayerOne.Char.CombatId = CombatHandler.NumberOfInstances();
            PlayerOne.Char.InCombatWith = PlayerTwo.UserId;

            PlayerTwo.Char.InCombat = true;
            PlayerTwo.Char.InPvpCombat = true;
            PlayerTwo.Char.CombatRequest = 0;
            PlayerTwo.Char.CombatId = CombatHandler.NumberOfInstances();
            PlayerTwo.Char.InCombatWith = PlayerOne.UserId;
        }

        public UserAccount GetPlayer(BasicMon mon)
        {
            if(PlayerOne.Char.HasMonInParty(mon))
                return PlayerOne;
            else
                return PlayerTwo;
        }

        public UserAccount GetOtherPlayer(UserAccount player)
        {
            if(player.UserId == PlayerOne.UserId)
                return PlayerTwo;
            else
                return PlayerOne;
        }

        public BasicMon GetOtherMon(BasicMon mon)
        {
            if(mon.OwnerID == PlayerOne.UserId)
            {
                return PlayerTwo.Char.ActiveMon;
            }
            else
            {
                return PlayerOne.Char.ActiveMon;
            }
        }

    }
}