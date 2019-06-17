using System.Collections;
using System.Threading.Tasks;

namespace DiscomonProject
{
    internal class Program
    {
        private static async Task Main()
        {
            var bot = Unity.Resolve<Discomon>();
            await bot.Start();
        }
    }

    
}
