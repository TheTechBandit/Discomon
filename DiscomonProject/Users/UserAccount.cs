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
        
    }
}