using CSharpWars.Logic.DependencyInjection;
using CSharpWars.ScriptProcessor.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpWars.ScriptProcessor.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureScriptProcessor(this IServiceCollection serviceCollection)
        {
            serviceCollection.ConfigureLogic();
            serviceCollection.AddSingleton<IBotScriptCache, BotScriptCache>();
            serviceCollection.AddTransient<IProcessor, Processor>();
        }
    }
}