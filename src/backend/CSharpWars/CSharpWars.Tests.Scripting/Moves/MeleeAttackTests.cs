using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;
using CSharpWars.ScriptProcessor.Moves;
using FluentAssertions;
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
            var botProperties = BotProperties.Build(bot, arena);
            botProperties.CurrentMove = PossibleMoves.MeleeAttack;

            // Act
            var move = Move.Build(botProperties);

            // Assert
            move.Should().NotBeNull();
            move.Should().BeOfType<MeleeAttack>();
        }
    }
}