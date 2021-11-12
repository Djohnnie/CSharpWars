using CSharpWars.Logging;
using CSharpWars.Logging.DependencyInjection;
using CSharpWars.Logging.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpWars.Tests.Logger.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    [Theory]
    [InlineData(typeof(ILogger), typeof(ConsoleLogger))]
    public void ConfigureLogging_Should_Register_Singleton_Logger_Classes(Type repositoryType, Type repositoryImplementation)
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.ConfigureLogging();
        var provider = serviceCollection.BuildServiceProvider();

        // Act
        var logger1 = provider.GetService(repositoryType);
        var logger2 = provider.GetService(repositoryType);

        // Assert
        logger1.Should().BeOfType(repositoryImplementation);
        logger2.Should().BeOfType(repositoryImplementation);
        logger1.Should().Be(logger2);
    }
}