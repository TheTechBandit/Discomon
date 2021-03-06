using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscomonProject.Discord;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class UserAccount
    {
        public ulong UserId { get; set; }
        public ulong DmId { get; set; }
        public string Mention { get; set; }
        public string Name { get; set; }
        public Character Char { get; set; }
        public string AvatarUrl { get; set; }
        public bool HasCharacter { get; set; }
        public int PromptState { get; set; }
        /* meaning behind each int
        0- attack screen main
        1- move selection screen
        2- attack screen rework
        3- move selection screen rework
        4- targeting screen rework
         */
        public Dictionary<ulong, int> ReactionMessages { get; set; }
        //ulong- Message ID. ulong- User ID. Compare with ReactionMessages to decide what type of invite it is
        public Dictionary<ulong, ulong> InviteMessages { get; set; }
        /*
        0- Invite player(s) to team
        1- Kick player(s) from team
        2- Change team name
        5- Join open team
        */
        public int ExpectedInput { get; set; }
        public ulong ExpectedInputLocation { get; set; }

        public UserAccount()
        {

        }
        
        public UserAccount(bool newuser)
        {
            HasCharacter = false;
            PromptState = -1;
            ReactionMessages = new Dictionary<ulong, int>();
            InviteMessages = new Dictionary<ulong, ulong>();
            ExpectedInput = -1;
            ExpectedInputLocation = 0;
        }

        public void RemoveAllReactionMessages(int type)
        {
            if(ReactionMessages.ContainsValue(type))
            {
                foreach(var item in ReactionMessages.Where(kvp => kvp.Value == type).ToList())
                {
                    ReactionMessages.Remove(item.Key);
                }
            }
        }

        public Team GetTeam()
        {
            if(Char != null)
                return TownHandler.GetTown(Char.CurrentGuildId).GetTeam(this);
            return null;
        }

        public string DebugString()
        {
            string str = $"UserID: {UserId}\nDmId: {DmId}\nMention: {Mention}\nName: {Name}\nAvatarUrl: {AvatarUrl}\nPromptState: {PromptState}";
            str += "\nReaction Messages- ";
            foreach(KeyValuePair<ulong, int> pair in ReactionMessages)
            {
                str += $"\nKey: {pair.Key} Value: {pair.Value}";
            }

            str += $"\nHasCharacter: {HasCharacter}";
            if(HasCharacter) str += $"\n**CHARACTER**\nName: {Char.Name}\nCurrentGuildName: {Char.CurrentGuildName}\nCurrentGuildId: {Char.CurrentGuildId}\nCombatRequest: {Char.CombatRequest}\nInCombat: {Char.InCombat}\nInPvpCombat: {Char.InPvpCombat}\nInCombatWith: {Char.InCombatWith}\nCombatId: {Char.CombatId}";
            return str;
        }
        
    }
}