using Discord.WebSocket;

namespace DiscomonProject
{
    public class DiscordUser
    {
        public SocketUser _user {get; set;}

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