using CSharpWars.Common.DependencyInjection;
using CSharpWars.Logging.DependencyInjection;
using CSharpWars.Logic.DependencyInjection;
using CSharpWars.ScriptProcessor.Interfaces;
using CSharpWars.ScriptProcessor.Middleware;
using CSharpWars.ScriptProcessor.Middleware.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpWars.ScriptProcessor.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureScriptProcessor(this IServiceCollection serviceCollection)
        {
            serviceCollection.ConfigureCommon();
            serviceCollection.ConfigureLogging();
            serviceCollection.ConfigureLogic();
            serviceCollection.AddSingleton<IBotScriptCache, BotScriptCache>();
            serviceCollection.AddSingleton<IMiddleware, Middleware.Middleware>();
            serviceCollection.AddSingleton<IPreprocessor, Preprocessor>();
            serviceCollection.AddSingleton<IProcessor, Processor>();
            serviceCollection.AddSingleton<IPostprocessor, Postprocessor>();
            serviceCollection.AddSingleton<IBotProcessingFactory, BotProcessingFactory>();
            serviceCollection.AddSingleton<IBotScriptCompiler, BotScriptCompiler>();
        }
    }
}