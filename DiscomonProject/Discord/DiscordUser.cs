using Discord.WebSocket;

namespace DiscomonProject
{
    public class DiscordUser
    {
        private SocketUser _user;

        public DiscordUser(SocketUser user)
        {
            _user = user;
        }

        public SocketUser GetUser()
        {
            return _user;
        }

        public ulong GetID()
        {
            return _user.Id;
        }
    }
}