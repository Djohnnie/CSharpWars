using System.Collections.Generic;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;
using CSharpWars.Processor.Moves;
using FluentAssertions;
using Moq;
using Xunit;

namespace CSharpWars.Tests.Scripting.Moves
{
    public class TurnLeftTests
    {
        [Fact]
        public void Building_A_Move_From_TurningLeft_Move_Should_Create_An_Instance_Of_TurnLeft()
        {
            // Arrange
            var bot = new BotDto { };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.TurningLeft;
            var randomHelper = new Mock<IRandomHelper>();

            // Act
            var move = Move.Build(botProperties, randomHelper.Object);

            // Assert
            move.Should().NotBeNull();
            move.Should().BeOfType<TurnLeft>();
        }

        [Theory]
        [ClassData(typeof(TurningLeftTheoryData))]
        public void Turning_Left_Should_Work(PossibleOrientations originOrientation, PossibleOrientations destinationOrientation)
        {
            // Arrange
            var bot = new BotDto
            {
                Orientation = originOrientation
            };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.TurningLeft;
            var randomHelper = new Mock<IRandomHelper>();
            var move = Move.Build(botProperties, randomHelper.Object);

            // Act
            var botResult = move.Go();

            // Assert
            botResult.Should().NotBeNull();
            botResult.Move.Should().Be(PossibleMoves.TurningLeft);
            botResult.Orientation.Should().Be(destinationOrientation);
        }

        private class TurningLeftTheoryData : TheoryData<PossibleOrientations, PossibleOrientations>
        {
            public TurningLeftTheoryData()
            {
                Add(PossibleOrientations.North, PossibleOrientations.West);
                Add(PossibleOrientations.East, PossibleOrientations.North);
                Add(PossibleOrientations.South, PossibleOrientations.East);
                Add(PossibleOrientations.West, PossibleOrientations.South);
            }
        }
    }
}