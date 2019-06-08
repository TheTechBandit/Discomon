namespace DiscomonProject
{
    public class UserAccount
    {
        private DiscordUser _user;
        public ulong id;
        public string name;

        public UserAccount(DiscordUser user)
        {
            _user = user;
            id = _user.GetID();
            name = _user.GetUser().Username;
        }

        public ulong getID()
        {
            return id;
        }
    }
}