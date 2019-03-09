using System.Collections.Generic;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;
using CSharpWars.ScriptProcessor.Moves;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Scripting.Moves
{
    public class TurnAroundTests
    {
        [Fact]
        public void Building_A_Move_From_TurningAround_Move_Should_Create_An_Instance_Of_TurnAround()
        {
            // Arrange
            var bot = new BotDto { };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.TurningAround;

            // Act
            var move = Move.Build(botProperties);

            // Assert
            move.Should().NotBeNull();
            move.Should().BeOfType<TurnAround>();
        }

        [Theory]
        [ClassData(typeof(TurningAroundTheoryData))]
        public void Turning_Around_Should_Work(PossibleOrientations originOrientation, PossibleOrientations destinationOrientation)
        {
            // Arrange
            var bot = new BotDto
            {
                Orientation = originOrientation
            };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.TurningAround;
            var move = Move.Build(botProperties);

            // Act
            var botResult = move.Go();

            // Assert
            botResult.Should().NotBeNull();
            botResult.CurrentMove.Should().Be(PossibleMoves.TurningAround);
            botResult.Orientation.Should().Be(destinationOrientation);
        }

        private class TurningAroundTheoryData : TheoryData<PossibleOrientations, PossibleOrientations>
        {
            public TurningAroundTheoryData()
            {
                Add(PossibleOrientations.North, PossibleOrientations.South);
                Add(PossibleOrientations.East, PossibleOrientations.West);
                Add(PossibleOrientations.South, PossibleOrientations.North);
                Add(PossibleOrientations.West, PossibleOrientations.East);
            }
        }
    }
}