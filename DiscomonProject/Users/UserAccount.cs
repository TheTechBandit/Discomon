using System;
using System.Collections;
using System.Collections.Generic;
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
         */
        public Dictionary<ulong, int> ReactionMessages { get; set; }

        public UserAccount()
        {

        }
        public UserAccount(bool newuser)
        {
            HasCharacter = false;
            PromptState = -1;
            Dictionary<ulong, int> ReactionMessages = new Dictionary<ulong, int>();
        }

        public string DebugString()
        {
            string str = $"UserID: {UserId}\nMention: {Mention}\nName: {Name}\nAvatarUrl: {AvatarUrl}\nPromptState: {PromptState}\nHasCharacter: {HasCharacter}";
            if(HasCharacter) str += $"\n**CHARACTER**\nName: {Char.Name}\nCurrentGuildName: {Char.CurrentGuildName}\nCurrentGuildId: {Char.CurrentGuildId}\nCombatRequest: {Char.CombatRequest}\nInCombat: {Char.InCombat}\nInPvpCombat: {Char.InPvpCombat}\nInCombatWith: {Char.InCombatWith}";
            if(Char.InCombat) str += $"\n**COMBAT INSTANCE**\nThisPlayer: {Char.Combat.ThisPlayer}\nOtherPlayer: {Char.Combat.OtherPlayer}\n";
            return str;
        }
        
    }
}