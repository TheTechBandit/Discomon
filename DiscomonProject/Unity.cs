using DiscomonProject.Discord;
using DiscomonProject.Storage;
using DiscomonProject.Storage.Implementations;
using Discord.Commands;
using Discord.WebSocket;
using Unity;
using Unity.Injection;
using Unity.Resolution;

namespace DiscomonProject
{
    public static class Unity
    {
        private static UnityContainer _container;

        public static UnityContainer Container
        {
            get
            {
                if(_container == null)
                    RegisterTypes();
                return _container;
            }
        }

        public static void RegisterTypes()
        {
            _container = new UnityContainer();
            _container.RegisterSingleton<IDataStorage, JsonStorage>();
            _container.RegisterSingleton<ILogger, Logger>();
            _container.RegisterType<DiscordSocketConfig>(new InjectionFactory(i => SocketConfig.GetDefault()));
            _container.RegisterSingleton<DiscordSocketClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
            _container.RegisterSingleton<CommandServiceConfig>(new InjectionFactory(i => CommandServConfig.GetDefault()));
            _container.RegisterSingleton<CommandService>(new InjectionConstructor(typeof(CommandServiceConfig)));
            _container.RegisterSingleton<Connection>();
        }

        public static T Resolve<T>() 
            => (T)Container.Resolve(typeof(T), string.Empty, new CompositeResolverOverride());
    }
}
