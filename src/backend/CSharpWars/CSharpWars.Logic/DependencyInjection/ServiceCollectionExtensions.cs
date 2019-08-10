using CSharpWars.Common.DependencyInjection;
using CSharpWars.DataAccess.DependencyInjection;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Mapping.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpWars.Logic.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureLogic(this IServiceCollection serviceCollection)
        {
            serviceCollection.ConfigureDataAccess();
            serviceCollection.ConfigureMapping();
            serviceCollection.ConfigureCommon();
            serviceCollection.AddTransient<IArenaLogic, ArenaLogic>();
            serviceCollection.AddTransient<IPlayerLogic, PlayerLogic>();
            serviceCollection.AddTransient<IBotLogic, BotLogic>();
            serviceCollection.AddTransient<IDangerLogic, DangerLogic>();
            serviceCollection.AddTransient<IMessageLogic, MessageLogic>();
        }
    }
}