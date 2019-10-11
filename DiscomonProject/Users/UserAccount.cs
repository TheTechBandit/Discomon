using System.Collections.Generic;
using System.Linq;

namespace DiscomonProject.Users
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
         */
        public Dictionary<ulong, int> ReactionMessages { get; set; }

        public UserAccount()
        {
            HasCharacter = false;
            PromptState = -1;
            ReactionMessages = new Dictionary<ulong, int>();
        }

        public string DebugString()
        {
            var str = $"UserID: {UserId}\nDmId: {DmId}\nMention: {Mention}\nName: {Name}\nAvatarUrl: {AvatarUrl}\nPromptState: {PromptState}";
            str += "\nReaction Messages- ";
            str = ReactionMessages.Aggregate(str, (current, pair) => current + $"\nKey: {pair.Key} Value: {pair.Value}");

            str += $"\nHasCharacter: {HasCharacter}";
            if(HasCharacter) str += $"\n**CHARACTER**\nName: {Char.Name}\nCurrentGuildName: {Char.CurrentGuildName}\nCurrentGuildId: {Char.CurrentGuildId}\nCombatRequest: {Char.CombatRequest}\nInCombat: {Char.InCombat}\nInPvpCombat: {Char.InPvpCombat}\nInCombatWith: {Char.InCombatWith}";
            if(Char.InCombat) str += $"\n**COMBAT INSTANCE**\nThisPlayer: {Char.Combat.ThisPlayer}\nOtherPlayer: {Char.Combat.OtherPlayer}\n";
            return str;
        }
        
    }
}