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
    public class MeleeAttackTests
    {
        [Fact]
        public void Building_A_Move_From_MeleeAttack_Move_Should_Create_An_Instance_Of_MeleeAttack()
        {
            // Arrange
            var bot = new BotDto { };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.MeleeAttack;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var move = Move.Build(botProperties, randomHelper.Object);

            // Assert
            move.Should().NotBeNull();
            move.Should().BeOfType<MeleeAttack>();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 0)]
        [InlineData(0, 1)]
        [InlineData(2, 1)]
        [InlineData(0, 2)]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        public void Executing_A_MeleeAttack_Into_Thin_Air_Should_Not_Inflict_Damage(Int32 victimX, Int32 victimY)
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100, X = 1, Y = 1, Orientation = PossibleOrientations.North };
            var victim = new BotDto { Id = Guid.NewGuid(), CurrentHealth = 100, X = victimX, Y = victimY };
            var arena = new ArenaDto { Width = 3, Height = 3 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>(new[] { victim }));
            botProperties.CurrentMove = PossibleMoves.MeleeAttack;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.GetInflictedDamage(victim.Id).Should().BeZero();
        }

        [Theory]
        [InlineData(PossibleOrientations.East)]
        [InlineData(PossibleOrientations.South)]
        [InlineData(PossibleOrientations.West)]
        public void Executing_A_MeleeAttack_To_A_Victim_Should_Inflict_Normal_Damage(PossibleOrientations victimOrientation)
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100, X = 1, Y = 1, Orientation = PossibleOrientations.North };
            var victim = new BotDto { Id = Guid.NewGuid(), CurrentHealth = 100, X = 1, Y = 0, Orientation = victimOrientation };
            var arena = new ArenaDto { Width = 3, Height = 3 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>(new[] { victim }));
            botProperties.CurrentMove = PossibleMoves.MeleeAttack;
            var expectedDamage = Constants.MELEE_DAMAGE;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.GetInflictedDamage(victim.Id).Should().Be(expectedDamage);
        }

        [Fact]
        public void Executing_A_MeleeAttack_To_A_Victim_That_Has_His_Back_To_Us_Should_Inflict_Extra_Damage()
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100, X = 1, Y = 1, Orientation = PossibleOrientations.North };
            var victim = new BotDto { Id = Guid.NewGuid(), CurrentHealth = 100, X = 1, Y = 0, Orientation = PossibleOrientations.North };
            var arena = new ArenaDto { Width = 3, Height = 3 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>(new[] { victim }));
            botProperties.CurrentMove = PossibleMoves.MeleeAttack;
            var expectedDamage = Constants.MELEE_BACKSTAB_DAMAGE;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.GetInflictedDamage(victim.Id).Should().Be(expectedDamage);
        }

        [Theory]
        [InlineData(1, 0, PossibleOrientations.North)]
        [InlineData(2, 1, PossibleOrientations.East)]
        [InlineData(1, 2, PossibleOrientations.South)]
        [InlineData(0, 1, PossibleOrientations.West)]
        public void Executing_A_MeleeAttack_Should_Work_In_The_Four_Possible_Orientations(Int32 victimX, Int32 victimY, PossibleOrientations orientation)
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100, X = 1, Y = 1, Orientation = orientation };
            var victim = new BotDto { Id = Guid.NewGuid(), CurrentHealth = 100, X = victimX, Y = victimY, Orientation = orientation };
            var arena = new ArenaDto { Width = 3, Height = 3 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>(new[] { victim }));
            botProperties.CurrentMove = PossibleMoves.MeleeAttack;
            var expectedDamage = Constants.MELEE_BACKSTAB_DAMAGE;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.GetInflictedDamage(victim.Id).Should().Be(expectedDamage);
        }
    }
}