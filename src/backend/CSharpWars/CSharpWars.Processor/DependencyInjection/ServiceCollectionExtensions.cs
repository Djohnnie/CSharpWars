using CSharpWars.Common.DependencyInjection;
using CSharpWars.Logging.DependencyInjection;
using CSharpWars.Logic.DependencyInjection;
using CSharpWars.Processor.Middleware;
using CSharpWars.Processor.Middleware.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpWars.Processor.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureScriptProcessor(this IServiceCollection serviceCollection)
        {
            serviceCollection.ConfigureCommon();
            serviceCollection.ConfigureLogging();
            serviceCollection.ConfigureLogic();
            serviceCollection.AddSingleton<IBotScriptCache, BotScriptCache>();
            serviceCollection.AddScoped<IMiddleware, Processor.Middleware.Middleware>();
            serviceCollection.AddScoped<IPreprocessor, Preprocessor>();
            serviceCollection.AddScoped<IProcessor, Middleware.Processor>();
            serviceCollection.AddScoped<IPostprocessor, Postprocessor>();
            serviceCollection.AddScoped<IBotProcessingFactory, BotProcessingFactory>();
            serviceCollection.AddScoped<IBotScriptCompiler, BotScriptCompiler>();
        }
    }
}