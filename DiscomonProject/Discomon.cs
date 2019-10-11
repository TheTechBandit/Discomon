using System.Threading.Tasks;
using DiscomonProject.Discord;
using DiscomonProject.Discord.Entities;
using DiscomonProject.Storage;

namespace DiscomonProject
{
    public class Discomon
    {
        private readonly IDataStorage _storage;
        private readonly Connection _connection;

        public Discomon(IDataStorage storage, Connection connection)
        {
            _storage = storage;
            _connection = connection;
        }

        public async Task Start()
        {
            await _connection.ConnectAsync(new MonBotConfig
            {
                Token = _storage.RestoreObject<string>("Config/BotToken")
            });
        }
    }
}