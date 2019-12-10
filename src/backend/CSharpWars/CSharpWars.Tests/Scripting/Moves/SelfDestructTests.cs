using System;
using System.Collections.Generic;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;
using CSharpWars.Processor.Moves;
using CSharpWars.Tests.Framework.FluentAssertions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CSharpWars.Tests.Scripting.Moves
{
    public class SelfDestructTests
    {
        [Fact]
        public void Building_A_Move_From_SelfDestruct_Move_Should_Create_An_Instance_Of_SelfDestruct()
        {
            // Arrange
            var bot = new BotDto { };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.SelfDestruct;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var move = Move.Build(botProperties, randomHelper.Object);

            // Assert
            move.Should().NotBeNull();
            move.Should().BeOfType<SelfDestruct>();
        }


        [Fact]
        public void Executing_A_SelfDestruct_Should_Kill_The_Bot()
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100 };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.SelfDestruct;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.Move.Should().Be(PossibleMoves.SelfDestruct);
            result.CurrentHealth.Should().BeZero();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(3, 0)]
        [InlineData(4, 0)]
        [InlineData(5, 0)]
        [InlineData(6, 0)]
        [InlineData(0, 1)]
        [InlineData(6, 1)]
        [InlineData(0, 2)]
        [InlineData(6, 2)]
        [InlineData(0, 3)]
        [InlineData(6, 3)]
        [InlineData(0, 4)]
        [InlineData(6, 4)]
        [InlineData(0, 5)]
        [InlineData(6, 5)]
        [InlineData(0, 6)]
        [InlineData(1, 6)]
        [InlineData(2, 6)]
        [InlineData(3, 6)]
        [InlineData(4, 6)]
        [InlineData(5, 6)]
        [InlineData(6, 6)]
        public void Executing_A_SelfDestruct_Should_Damage_Minimum_Vicinity_Bots_With_Minimum_Damage(Int32 victimX, Int32 victimY)
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100, X = 3, Y = 3 };
            var victim = new BotDto { Id = Guid.NewGuid(), CurrentHealth = 100, X = victimX, Y = victimY };
            var arena = new ArenaDto { Width = 7, Height = 7 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>(new[] { victim }));
            botProperties.CurrentMove = PossibleMoves.SelfDestruct;
            var expectedDamage = Constants.SELF_DESTRUCT_MIN_DAMAGE;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.GetInflictedDamage(victim.Id).Should().Be(expectedDamage);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(3, 0)]
        [InlineData(4, 0)]
        [InlineData(0, 1)]
        [InlineData(4, 1)]
        [InlineData(0, 2)]
        [InlineData(4, 2)]
        [InlineData(0, 3)]
        [InlineData(4, 3)]
        [InlineData(0, 4)]
        [InlineData(1, 4)]
        [InlineData(2, 4)]
        [InlineData(3, 4)]
        [InlineData(4, 4)]
        public void Executing_A_SelfDestruct_Should_Damage_Medium_Vicinity_Bots_With_Medium_Damage(Int32 victimX, Int32 victimY)
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100, X = 2, Y = 2 };
            var victim = new BotDto { Id = Guid.NewGuid(), CurrentHealth = 100, X = victimX, Y = victimY };
            var arena = new ArenaDto { Width = 5, Height = 5 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>(new[] { victim }));
            botProperties.CurrentMove = PossibleMoves.SelfDestruct;
            var expectedDamage = Constants.SELF_DESTRUCT_MED_DAMAGE + Constants.SELF_DESTRUCT_MIN_DAMAGE;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.GetInflictedDamage(victim.Id).Should().Be(expectedDamage);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(0, 1)]
        [InlineData(2, 1)]
        [InlineData(0, 2)]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        public void Executing_A_SelfDestruct_Should_Damage_Maximum_Vicinity_Bots_With_Maximum_Damage(Int32 victimX, Int32 victimY)
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100, X = 1, Y = 1 };
            var victim = new BotDto { Id = Guid.NewGuid(), CurrentHealth = 100, X = victimX, Y = victimY };
            var arena = new ArenaDto { Width = 3, Height = 3 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>(new[] { victim }));
            botProperties.CurrentMove = PossibleMoves.SelfDestruct;
            var expectedDamage = Constants.SELF_DESTRUCT_MAX_DAMAGE + Constants.SELF_DESTRUCT_MED_DAMAGE + Constants.SELF_DESTRUCT_MIN_DAMAGE;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.GetInflictedDamage(victim.Id).Should().Be(expectedDamage);
        }
    }
}