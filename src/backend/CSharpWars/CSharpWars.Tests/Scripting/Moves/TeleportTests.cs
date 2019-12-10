using System;
using System.Collections.Generic;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;
using CSharpWars.Processor.Moves;
using CSharpWars.Tests.Framework.FluentAssertions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CSharpWars.Tests.Scripting.Moves
{
    public class TeleportTests
    {
        [Fact]
        public void Building_A_Move_From_Teleport_Move_Should_Create_An_Instance_Of_Teleport()
        {
            // Arrange
            var bot = new BotDto { };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.Teleport;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var move = Move.Build(botProperties, randomHelper.Object);

            // Assert
            move.Should().NotBeNull();
            move.Should().BeOfType<Teleport>();
        }

        [Fact]
        public void Executing_A_Teleport_Into_An_Empty_Spot_Should_Work()
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100, X = 0, Y = 0, Orientation = PossibleOrientations.North, CurrentStamina = 10 };
            var arena = new ArenaDto { Width = 5, Height = 2 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.Teleport;
            botProperties.MoveDestinationX = 4;
            botProperties.MoveDestinationY = 1;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.X.Should().Be(botProperties.MoveDestinationX);
            result.Y.Should().Be(botProperties.MoveDestinationY);
            result.Move.Should().Be(PossibleMoves.Teleport);
        }

        [Theory]
        [InlineData(4, -1)]
        [InlineData(5, 0)]
        [InlineData(4, 1)]
        [InlineData(-1, 0)]
        public void Executing_A_Teleport_Into_An_Invalid_Spot_Should_Randomly_Teleport(Int32 destinationX, Int32 destinationY)
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100, X = 0, Y = 0, Orientation = PossibleOrientations.North, CurrentStamina = 10 };
            var arena = new ArenaDto { Width = 5, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.Teleport;
            botProperties.MoveDestinationX = destinationX;
            botProperties.MoveDestinationY = destinationY;
            var randomHelper = new Mock<IRandomHelper>();
            var randomX = 4;
            var randomY = 0;

            // Mock
            randomHelper.Setup(x => x.Get(It.Is<Int32>(w => w == arena.Width))).Returns(randomX);
            randomHelper.Setup(x => x.Get(It.Is<Int32>(w => w == arena.Height))).Returns(randomY);

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.X.Should().Be(randomX);
            result.Y.Should().Be(randomY);
            result.Move.Should().Be(PossibleMoves.Teleport);
        }

        [Fact]
        public void Executing_A_Teleport_Into_An_Enemy_Should_Switch_Places()
        {
            // Arrange
            var bot = new BotDto { CurrentHealth = 100, X = 0, Y = 1, Orientation = PossibleOrientations.North, CurrentStamina = 10 };
            var victim = new BotDto { Id = Guid.NewGuid(), CurrentHealth = 100, X = 4, Y = 0 };
            var arena = new ArenaDto { Width = 5, Height = 2 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>(new[] { victim }));
            botProperties.CurrentMove = PossibleMoves.Teleport;
            botProperties.MoveDestinationX = 4;
            botProperties.MoveDestinationY = 0;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var result = Move.Build(botProperties, randomHelper.Object).Go();

            // Assert
            result.Should().NotBeNull();
            result.X.Should().Be(botProperties.MoveDestinationX);
            result.Y.Should().Be(botProperties.MoveDestinationY);
            result.GetTeleportation(victim.Id).X.Should().Be(bot.X);
            result.GetTeleportation(victim.Id).Y.Should().Be(bot.Y);
            result.Move.Should().Be(PossibleMoves.Teleport);
        }
    }
}