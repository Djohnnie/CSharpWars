using CSharpWars.Common.Configuration.Interfaces;
using CSharpWars.Common.DependencyInjection;
using CSharpWars.Common.Helpers;
using CSharpWars.Common.Helpers.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CSharpWars.Tests.Common.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ConfigurationHelper_Should_Configure_A_ConfigurationHelper()
        {
            // Arrange
            var arenaSize = 10;
            var connectionString = "CONNECTION-STRING";
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.ConfigurationHelper(c =>
            {
                c.ArenaSize = arenaSize;
                c.ConnectionString = connectionString;
            });
            var provider = serviceCollection.BuildServiceProvider();
            var configurationHelper = provider.GetService<IConfigurationHelper>();

            // Assert
            configurationHelper.ArenaSize.Should().Be(arenaSize);
            configurationHelper.ConnectionString.Should().Be(connectionString);
        }

        [Fact]
        public void ConfigureCommon_Should_Register_A_Singleton_ConfigurationHelper()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.ConfigureCommon();
            var provider = serviceCollection.BuildServiceProvider();
            var randomHelper1 = provider.GetService<IRandomHelper>();
            var randomHelper2 = provider.GetService<IRandomHelper>();

            // Assert
            randomHelper1.Should().BeOfType<RandomHelper>();
            randomHelper2.Should().BeOfType<RandomHelper>();
            randomHelper1.Should().Be(randomHelper2);
        }
    }
}