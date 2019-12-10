using System;
using System.Collections.Generic;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;
using CSharpWars.Processor.Moves;
using FluentAssertions;
using Moq;
using Xunit;

namespace CSharpWars.Tests.Scripting.Moves
{
    public class WalkForwardTests
    {
        [Fact]
        public void Building_A_Move_From_WalkForward_Move_Should_Create_An_Instance_Of_WalkForward()
        {
            // Arrange
            var bot = new BotDto { };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.WalkForward;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var move = Move.Build(botProperties, randomHelper.Object);

            // Assert
            move.Should().NotBeNull();
            move.Should().BeOfType<WalkForward>();
        }

        [Fact]
        public void Walking_Forward_Without_Stamina_Should_Be_Ignored()
        {
            // Arrange
            var bot = new BotDto { X = 1, Y = 1 };
            var arena = new ArenaDto { Width = 3, Height = 3 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.WalkForward;
            var randomHelper = new Mock<IRandomHelper>();
            var move = Move.Build(botProperties, randomHelper.Object);

            // Act
            var botResult = move.Go();

            // Assert
            botResult.Should().NotBeNull();
            botResult.X.Should().Be(1);
            botResult.Y.Should().Be(1);
            botResult.Move.Should().Be(PossibleMoves.Idling);
            botResult.CurrentStamina.Should().Be(0);
        }

        [Theory]
        [ClassData(typeof(WalkForwardOrientationTheoryData))]
        public void Walking_Forward_Against_Edge_Should_Be_Ignored(PossibleOrientations orientation)
        {
            // Arrange
            var bot = new BotDto
            {
                Orientation = orientation,
                CurrentHealth = 100,
                CurrentStamina = 100
            };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.WalkForward;
            var randomHelper = new Mock<IRandomHelper>();
            var move = Move.Build(botProperties, randomHelper.Object);

            // Act
            var botResult = move.Go();

            // Assert
            botResult.Should().NotBeNull();
            botResult.X.Should().Be(0);
            botResult.Y.Should().Be(0);
            botResult.Move.Should().Be(PossibleMoves.Idling);
            botResult.CurrentStamina.Should().Be(bot.CurrentStamina);
        }

        private class WalkForwardOrientationTheoryData : TheoryData<PossibleOrientations>
        {
            public WalkForwardOrientationTheoryData()
            {
                Add(PossibleOrientations.North);
                Add(PossibleOrientations.East);
                Add(PossibleOrientations.South);
                Add(PossibleOrientations.West);
            }
        }

        [Theory]
        [ClassData(typeof(WalkForwardTheoryData))]
        private void Walking_Forward_Should_Work(PossibleOrientations orientation, Int32 destinationX, Int32 destinationY)
        {
            // Arrange
            var bot = new BotDto
            {
                X = 1,
                Y = 1,
                Orientation = orientation,
                CurrentHealth = 100,
                CurrentStamina = 100
            };
            var arena = new ArenaDto { Width = 3, Height = 3 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.WalkForward;
            var randomHelper = new Mock<IRandomHelper>();
            var move = Move.Build(botProperties, randomHelper.Object);

            // Act
            var botResult = move.Go();

            // Assert
            botResult.Should().NotBeNull();
            botResult.X.Should().Be(destinationX);
            botResult.Y.Should().Be(destinationY);
            botResult.Move.Should().Be(PossibleMoves.WalkForward);
            botResult.CurrentStamina.Should().Be(bot.CurrentStamina - Constants.STAMINA_ON_MOVE);
        }

        private class WalkForwardTheoryData : TheoryData<PossibleOrientations, Int32, Int32>
        {
            public WalkForwardTheoryData()
            {
                Add(PossibleOrientations.North, 1, 0);
                Add(PossibleOrientations.East, 2, 1);
                Add(PossibleOrientations.South, 1, 2);
                Add(PossibleOrientations.West, 0, 1);
            }
        }
    }
}