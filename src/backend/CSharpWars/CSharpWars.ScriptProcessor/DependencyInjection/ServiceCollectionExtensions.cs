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
            serviceCollection.AddScoped<IMiddleware, Middleware.Middleware>();
            serviceCollection.AddScoped<IPreprocessor, Preprocessor>();
            serviceCollection.AddScoped<IProcessor, Processor>();
            serviceCollection.AddScoped<IPostprocessor, Postprocessor>();
            serviceCollection.AddScoped<IBotProcessingFactory, BotProcessingFactory>();
            serviceCollection.AddScoped<IBotScriptCompiler, BotScriptCompiler>();
        }
    }
}