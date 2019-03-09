using CSharpWars.Common.DependencyInjection;
using CSharpWars.Logic;
using CSharpWars.Logic.DependencyInjection;
using CSharpWars.Logic.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CSharpWars.Tests.Logic.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ConfigureLogic_Should_Register_A_Transient_ArenaLogic()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.ConfigureLogic();
            serviceCollection.ConfigurationHelper(c =>
            {
                c.ArenaSize = 10;
                c.ConnectionString = "";
            });
            var provider = serviceCollection.BuildServiceProvider();
            var arenaLogic1 = provider.GetService<IArenaLogic>();
            var arenaLogic2 = provider.GetService<IArenaLogic>();

            // Assert
            arenaLogic1.Should().BeOfType<ArenaLogic>();
            arenaLogic2.Should().BeOfType<ArenaLogic>();
            arenaLogic1.Should().NotBe(arenaLogic2);
        }

        [Fact]
        public void ConfigureLogic_Should_Register_A_Transient_PlayerLogic()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.ConfigureLogic();
            serviceCollection.ConfigurationHelper(c =>
            {
                c.ArenaSize = 10;
                c.ConnectionString = "";
            });
            var provider = serviceCollection.BuildServiceProvider();
            var playerLogic1 = provider.GetService<IPlayerLogic>();
            var playerLogic2 = provider.GetService<IPlayerLogic>();

            // Assert
            playerLogic1.Should().BeOfType<PlayerLogic>();
            playerLogic2.Should().BeOfType<PlayerLogic>();
            playerLogic1.Should().NotBe(playerLogic2);
        }



        [Fact]
        public void ConfigureLogic_Should_Register_A_Transient_BotLogic()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.ConfigureLogic();
            serviceCollection.ConfigurationHelper(c =>
            {
                c.ArenaSize = 10;
                c.ConnectionString = "";
            });
            var provider = serviceCollection.BuildServiceProvider();
            var botLogic1 = provider.GetService<IBotLogic>();
            var botLogic2 = provider.GetService<IBotLogic>();

            // Assert
            botLogic1.Should().BeOfType<BotLogic>();
            botLogic2.Should().BeOfType<BotLogic>();
            botLogic1.Should().NotBe(botLogic2);
        }
    }
}