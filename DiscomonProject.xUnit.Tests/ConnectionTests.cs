using System.Threading.Tasks;
using DiscomonProject.Discord;
using DiscomonProject.Discord.Entities;
using Discord.Net;
using Xunit;

namespace DiscomonProject.xUnit.Tests
{
    public class ConnectionTests
    {
        [Fact]
        public void ConnectionAsyncTest()
        {
            Assert.ThrowsAsync<HttpException>(AttemptWrongConnect);
        }

        private async Task AttemptWrongConnect()
        {
            var conn = Unity.Resolve<Connection>();
            await conn.ConnectAsync(new MonBotConfig {Token = "FAKE-TOKEN"});
        }
    }
}