using System.Collections;
using DiscomonProject.Discord;

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
            HasCharacter = false;
            PromptState = -1;
        }

        public string DebugString()
        {
            string str = $"UserID: {UserId}\nMention: {Mention}\nName: {Name}\nAvatarUrl: {AvatarUrl}\nPromptState: {PromptState}\nHasCharacter: {HasCharacter}";
            if(HasCharacter) str += $"\n**CHARACTER**\nName: {Char.Name}\nCurrentGuildName: {Char.CurrentGuildName}\nCurrentGuildId: {Char.CurrentGuildId}\nCombatRequest: {Char.CombatRequest}\nInCombat: {Char.InCombat}\nInPvpCombat: {Char.InPvpCombat}\nInCombatWith: {Char.InCombatWith}";
            return str;
        }
        
    }
}