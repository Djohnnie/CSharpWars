using System;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Scripting
{
    public class ScriptGlobalsTests
    {
        [Fact]
        public void ScriptGlobals_Properties_Represent_BotProperties_Correctly()
        {
            // Arrange
            var botProperties = BuildBotProperties();

            // Act
            var scriptGlobals = new ScriptGlobals(botProperties);

            // Assert
            scriptGlobals.Should().NotBeNull();
            scriptGlobals.Should().BeEquivalentTo(botProperties, o => o
                .Including(x => x.Width)
                .Including(x => x.Height)
                .Including(x => x.X)
                .Including(x => x.Y)
                .Including(x => x.Orientation)
                .Including(x => x.LastMove)
                .Including(x => x.MaximumHealth)
                .Including(x => x.CurrentHealth)
                .Including(x => x.MaximumStamina)
                .Including(x => x.CurrentStamina));
        }

        [Theory]
        [ClassData(typeof(ScriptGlobalsSetsCurrentMoveTheoryData))]
        public void ScriptGlobals_Action_Correctly_Sets_CurrentMove_If_Idling(Action<ScriptGlobals> action, PossibleMoves expectedMove)
        {
            // Arrange
            var botProperties = BuildBotProperties();
            var scriptGlobals = new ScriptGlobals(botProperties);
            var originalMove = botProperties.CurrentMove;

            // Act
            action(scriptGlobals);

            // Assert
            originalMove.Should().Be(PossibleMoves.Idling);
            botProperties.CurrentMove.Should().Be(expectedMove);
        }

        [Theory]
        [ClassData(typeof(ScriptGlobalsIgnoresCurrentMoveTheoryData))]
        public void ScriptGlobals_Action_Correctly_Ignores_CurrentMove_If_Not_Idling(Action<ScriptGlobals> action, PossibleMoves originalMove)
        {
            // Arrange
            var botProperties = BuildBotProperties();
            botProperties.CurrentMove = originalMove;
            var scriptGlobals = new ScriptGlobals(botProperties);

            // Act
            action(scriptGlobals);

            // Assert
            originalMove.Should().NotBe(PossibleMoves.Idling);
            botProperties.CurrentMove.Should().Be(originalMove);
        }

        private BotProperties BuildBotProperties()
        {
            var bot = new BotDto
            {
                LocationX = 1,
                LocationY = 2,
                Orientation = PossibleOrientations.North,
                PreviousMove = PossibleMoves.Idling,
                MaximumHealth = 100,
                CurrentHealth = 99,
                MaximumStamina = 250,
                CurrentStamina = 150
            };
            var arena = new ArenaDto { Width = 10, Height = 20 };
            return BotProperties.Build(bot, arena);
        }

        private class ScriptGlobalsSetsCurrentMoveTheoryData : TheoryData<Action<ScriptGlobals>, PossibleMoves>
        {
            public ScriptGlobalsSetsCurrentMoveTheoryData()
            {
                Add(g => g.MoveForward(), PossibleMoves.WalkForward);
                Add(g => g.TurnLeft(), PossibleMoves.TurningLeft);
                Add(g => g.TurnRight(), PossibleMoves.TurningRight);
                Add(g => g.TurnAround(), PossibleMoves.TurningAround);
                Add(g => g.SelfDestruct(), PossibleMoves.SelfDestruct);
                Add(g => g.MeleeAttack(), PossibleMoves.MeleeAttack);
                Add(g => g.RangedAttack(0, 0), PossibleMoves.RangedAttack);
                Add(g => g.Teleport(0, 0), PossibleMoves.Teleport);
            }
        }

        private class ScriptGlobalsIgnoresCurrentMoveTheoryData : TheoryData<Action<ScriptGlobals>, PossibleMoves>
        {
            public ScriptGlobalsIgnoresCurrentMoveTheoryData()
            {
                Add(g => g.MoveForward(), PossibleMoves.MeleeAttack);
                Add(g => g.TurnLeft(), PossibleMoves.WalkForward);
                Add(g => g.TurnRight(), PossibleMoves.WalkForward);
                Add(g => g.TurnAround(), PossibleMoves.WalkForward);
                Add(g => g.SelfDestruct(), PossibleMoves.WalkForward);
                Add(g => g.MeleeAttack(), PossibleMoves.WalkForward);
                Add(g => g.RangedAttack(0, 0), PossibleMoves.WalkForward);
                Add(g => g.Teleport(0, 0), PossibleMoves.WalkForward);
            }
        }
    }
}