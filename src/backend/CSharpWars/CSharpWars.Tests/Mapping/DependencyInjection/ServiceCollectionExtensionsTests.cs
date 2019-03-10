using System;
using CSharpWars.DtoModel;
using CSharpWars.Mapping;
using CSharpWars.Mapping.DependencyInjection;
using CSharpWars.Mapping.Interfaces;
using CSharpWars.Model;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CSharpWars.Tests.Mapping.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        [Theory]
        [InlineData(typeof(IMapper<Player, PlayerDto>), typeof(PlayerMapper))]
        [InlineData(typeof(IMapper<Bot, BotDto>), typeof(BotMapper))]
        [InlineData(typeof(IMapper<Bot, BotToCreateDto>), typeof(BotToCreateMapper))]
        public void ConfigureMapping_Should_Register_Singleton_Mapper_Classes(Type mapperType, Type mapperImplementation)
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.ConfigureMapping();
            var provider = serviceCollection.BuildServiceProvider();
            var mapper1 = provider.GetService(mapperType);
            var mapper2 = provider.GetService(mapperType);

            // Assert
            mapper1.Should().BeOfType(mapperImplementation);
            mapper2.Should().BeOfType(mapperImplementation);
            mapper1.Should().Be(mapper2);
        }
    }
}