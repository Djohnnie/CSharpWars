using System.Collections.Generic;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;
using CSharpWars.ScriptProcessor.Moves;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Scripting.Moves
{
    public class TurnRightTests
    {
        [Fact]
        public void Building_A_Move_From_TurningRight_Move_Should_Create_An_Instance_Of_TurnRight()
        {
            // Arrange
            var bot = new BotDto { };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.TurningRight;

            // Act
            var move = Move.Build(botProperties);

            // Assert
            move.Should().NotBeNull();
            move.Should().BeOfType<TurnRight>();
        }

        [Theory]
        [ClassData(typeof(TurningRightTheoryData))]
        public void Turning_Right_Should_Work(PossibleOrientations originOrientation, PossibleOrientations destinationOrientation)
        {
            // Arrange
            var bot = new BotDto
            {
                Orientation = originOrientation
            };
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = PossibleMoves.TurningRight;
            var move = Move.Build(botProperties);

            // Act
            var botResult = move.Go();

            // Assert
            botResult.Should().NotBeNull();
            botResult.CurrentMove.Should().Be(PossibleMoves.TurningRight);
            botResult.Orientation.Should().Be(destinationOrientation);
        }

        private class TurningRightTheoryData : TheoryData<PossibleOrientations, PossibleOrientations>
        {
            public TurningRightTheoryData()
            {
                Add(PossibleOrientations.North, PossibleOrientations.East);
                Add(PossibleOrientations.East, PossibleOrientations.South);
                Add(PossibleOrientations.South, PossibleOrientations.West);
                Add(PossibleOrientations.West, PossibleOrientations.North);
            }
        }
    }
}