using CSharpWars.Logging.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpWars.Logging.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureLogging(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ILogger, ConsoleLogger>();
        }
    }
}