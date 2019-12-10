using System;
using System.Collections.Generic;
using CSharpWars.Common.Extensions;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;
using CSharpWars.Processor.Middleware;
using CSharpWars.Tests.Framework.FluentAssertions;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Scripting.Middleware
{
    public class BotResultTests
    {
        [Fact]
        public void BotResult_Build_From_BotProperties_Should_Copy_Properties()
        {
            // Arrange
            var bot = new BotDto
            {
                X = 1,
                Y = 2,
                Orientation = PossibleOrientations.East,
                CurrentHealth = 99,
                CurrentStamina = 999,
                Memory = new Dictionary<String, String>().Serialize()
            };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.MoveDestinationX = 3;
            botProperties.MoveDestinationY = 4;

            // Act
            var result = BotResult.Build(botProperties);

            // Assert
            result.Should().NotBeNull();
            result.X.Should().Be(bot.X);
            result.Y.Should().Be(bot.Y);
            result.Orientation.Should().Be(bot.Orientation);
            result.CurrentHealth.Should().Be(bot.CurrentHealth);
            result.CurrentStamina.Should().Be(bot.CurrentStamina);
            result.Memory.Should().BeEquivalentTo(bot.Memory.Deserialize<Dictionary<String, String>>());
            result.Messages.Should().BeEquivalentTo(new List<String>());
            result.Move.Should().Be(PossibleMoves.Idling);
            result.LastAttackX.Should().Be(botProperties.MoveDestinationX);
            result.LastAttackY.Should().Be(botProperties.MoveDestinationY);
        }

        [Fact]
        public void BotResult_InflictDamage_Once_Should_Correctly_Register_Damage()
        {
            // Arrange
            var bot = new BotDto();
            var arena = new ArenaDto();
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            var botResult = BotResult.Build(botProperties);
            var botId = Guid.NewGuid();
            var damage = 123;

            // Act
            botResult.InflictDamage(botId, damage);

            // Assert
            botResult.GetInflictedDamage(botId).Should().Be(damage);
        }

        [Fact]
        public void BotResult_InflictDamage_Twice_Should_Correctly_Register_All_Damage()
        {
            // Arrange
            var bot = new BotDto();
            var arena = new ArenaDto();
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            var botResult = BotResult.Build(botProperties);
            var botId = Guid.NewGuid();
            var firstDamage = 123;
            var secondDamage = 234;

            // Act
            botResult.InflictDamage(botId, firstDamage);
            botResult.InflictDamage(botId, secondDamage);

            // Assert
            botResult.GetInflictedDamage(botId).Should().Be(firstDamage + secondDamage);
        }

        [Fact]
        public void BotResult_GetInflictedDamage_Should_Return_Zero_For_Unknown_BotId()
        {
            // Arrange
            var bot = new BotDto();
            var arena = new ArenaDto();
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            var botResult = BotResult.Build(botProperties);

            // Act
            var result = botResult.GetInflictedDamage(Guid.NewGuid());

            // Assert
            result.Should().BeZero();
        }
    }
}