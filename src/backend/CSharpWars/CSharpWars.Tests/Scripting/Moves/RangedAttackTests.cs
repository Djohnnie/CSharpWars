using System.Collections.Generic;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;
using CSharpWars.ScriptProcessor.Moves;
using FluentAssertions;
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

            // Act
            var move = Move.Build(botProperties);

            // Assert
            move.Should().NotBeNull();
            move.Should().BeOfType<RangedAttack>();
        }
    }
}