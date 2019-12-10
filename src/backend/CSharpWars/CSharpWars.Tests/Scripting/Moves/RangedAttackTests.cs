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
    public class RangedAttackTests
    {
        [Fact]
        public void Building_A_Move_From_RangedAttack_Move_Should_Create_An_Instance_Of_RangedAttack()
        {
            // Arrange
            var bot = new BotDto { };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.RangedAttack;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var move = Move.Build(botProperties, randomHelper.Object);

            // Assert
            move.Should().NotBeNull();
            move.Should().BeOfType<RangedAttack>();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 0)]
        [InlineData(0, 1)]
        [InlineData(2, 1)]
        [InlineData(0, 2)]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        public void Executing_A_RangedAttack_Into_Thin_Air_Should_Not_Inflict_Damage(Int32 moveDestinationX, Int32 moveDestinationY)
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100, X = 1, Y = 1, Orientation = PossibleOrientations.North };
            var victim = new BotDto { Id = Guid.NewGuid(), CurrentHealth = 100, X = 1, Y = 0 };
            var arena = new ArenaDto { Width = 3, Height = 3 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>(new[] { victim }));
            botProperties.CurrentMove = PossibleMoves.RangedAttack;
            botProperties.MoveDestinationX = moveDestinationX;
            botProperties.MoveDestinationY = moveDestinationY;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.GetInflictedDamage(victim.Id).Should().BeZero();
        }

        [Fact]
        public void Executing_A_RangedAttack_To_A_Victim_Should_Inflict_Damage()
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100, X = 1, Y = 1, Orientation = PossibleOrientations.North };
            var victim = new BotDto { Id = Guid.NewGuid(), CurrentHealth = 100, X = 1, Y = 0, Orientation = PossibleOrientations.South };
            var arena = new ArenaDto { Width = 3, Height = 3 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>(new[] { victim }));
            botProperties.CurrentMove = PossibleMoves.RangedAttack;
            botProperties.MoveDestinationX = 1;
            botProperties.MoveDestinationY = 0;
            var expectedDamage = Constants.RANGED_DAMAGE;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.GetInflictedDamage(victim.Id).Should().Be(expectedDamage);
        }
    }
}