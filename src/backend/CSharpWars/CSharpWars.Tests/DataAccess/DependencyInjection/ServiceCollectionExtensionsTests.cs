using System;
using CSharpWars.Common.DependencyInjection;
using CSharpWars.DataAccess.DependencyInjection;
using CSharpWars.DataAccess.Repositories;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.Model;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CSharpWars.Tests.DataAccess.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        [Theory]
        [InlineData(typeof(IRepository<Player>), typeof(PlayerRepository))]
        [InlineData(typeof(IRepository<Bot>), typeof(BotRepository))]
        [InlineData(typeof(IRepository<BotScript>), typeof(ScriptRepository))]
        public void ConfigureDataAccess_Should_Register_Scoped_Repository_Classes(Type repositoryType, Type repositoryImplementation)
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.ConfigureDataAccess();
            serviceCollection.ConfigurationHelper(c =>
            {
                c.ArenaSize = 10;
                c.ConnectionString = "";
            });
            var provider = serviceCollection.BuildServiceProvider();

            // Act
            var repository1 = provider.GetService(repositoryType);
            var repository2 = provider.GetService(repositoryType);

            // Assert
            repository1.Should().BeOfType(repositoryImplementation);
            repository2.Should().BeOfType(repositoryImplementation);
            repository1.Should().Be(repository2);
        }
    }
}