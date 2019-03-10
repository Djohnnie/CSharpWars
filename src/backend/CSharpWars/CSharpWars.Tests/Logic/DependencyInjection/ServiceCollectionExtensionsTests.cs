using System;
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
        [Theory]
        [InlineData(typeof(IArenaLogic), typeof(ArenaLogic))]
        [InlineData(typeof(IPlayerLogic), typeof(PlayerLogic))]
        [InlineData(typeof(IBotLogic), typeof(BotLogic))]
        public void ConfigureLogic_Should_Register_Transient_Logic_Classes(Type logicType, Type logicImplementation)
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
            var logic1 = provider.GetService(logicType);
            var logic2 = provider.GetService(logicType);

            // Assert
            logic1.Should().BeOfType(logicImplementation);
            logic2.Should().BeOfType(logicImplementation);
            logic1.Should().NotBe(logic2);
        }
    }
}