using System;
using System.Collections;
using DiscomonProject.Discord;
using Newtonsoft.Json;

namespace DiscomonProject
{
    public class UserAccount
    {
        public ulong UserId { get; set; }
        public string Mention { get; set; }
        public string Name { get; set; }
        public Character Char { get; set; }
        public string AvatarUrl { get; set; }
        public bool HasCharacter { get; set; }
        public int PromptState { get; set; }

        public UserAccount()
        {

        }
        public UserAccount(bool newuser)
        {
            HasCharacter = false;
            PromptState = -1;
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