using System;
using Microsoft.Extensions.DependencyInjection;
using CSharpWars.Common.Configuration;
using CSharpWars.Common.Configuration.Interfaces;
using CSharpWars.Common.Helpers;
using CSharpWars.Common.Helpers.Interfaces;

namespace CSharpWars.Common.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigurationHelper(this IServiceCollection serviceCollection, Action<IConfigurationHelper> configuration)
        {
            serviceCollection.AddSingleton<IConfigurationHelper, ConfigurationHelper>(factory =>
            {
                var configurationHelper = new ConfigurationHelper();
                configuration(configurationHelper);
                return configurationHelper;
            });
        }

        public static void ConfigureCommon(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IRandomHelper, RandomHelper>();
        }
    }
}