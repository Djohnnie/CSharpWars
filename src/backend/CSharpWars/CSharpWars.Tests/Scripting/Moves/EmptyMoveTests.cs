using System;
using System.Collections.Generic;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;
using CSharpWars.ScriptProcessor.Moves;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Scripting.Moves
{
    public class EmptyMoveTests
    {
        [Theory]
        [InlineData(PossibleMoves.Idling)]
        [InlineData(PossibleMoves.ScriptError)]
        public void Building_A_Move_From_Unsupported_Move_Should_Create_An_Instance_Of_EmptyMove(PossibleMoves unsupportedMove)
        {
            // Arrange
            var bot = new BotDto();
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = unsupportedMove;

            // Act
            var move = Move.Build(botProperties);

            // Assert
            move.Should().NotBeNull();
            move.Should().BeOfType<EmptyMove>();
        }

        [Theory]
        [InlineData(PossibleMoves.Idling)]
        [InlineData(PossibleMoves.ScriptError)]
        public void Executing_An_Unsupported_Move_Should_Do_Nothing(PossibleMoves unsupportedMove)
        {
            // Arrange
            var bot = new BotDto();
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var botProperties = BotProperties.Build(bot, arena, new List<BotDto>());
            botProperties.CurrentMove = unsupportedMove;

            // Act
            var result = Move.Build(botProperties).Go();

            // Assert
            result.Should().NotBeNull();
            result.Move.Should().Be(unsupportedMove);
        }
    }
}