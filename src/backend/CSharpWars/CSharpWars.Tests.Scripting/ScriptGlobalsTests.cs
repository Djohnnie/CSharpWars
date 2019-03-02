using System;
using System.Collections.Generic;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;
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
            Assert.Equal(botProperties.Width, scriptGlobals.Width);
            Assert.Equal(botProperties.Height, scriptGlobals.Height);
            Assert.Equal(botProperties.X, scriptGlobals.X);
            Assert.Equal(botProperties.Y, scriptGlobals.Y);
            Assert.Equal(botProperties.Orientation, scriptGlobals.Orientation);
            Assert.Equal(botProperties.LastMove, scriptGlobals.LastMove);
            Assert.Equal(botProperties.MaximumPhysicalHealth, scriptGlobals.MaximumPhysicalHealth);
            Assert.Equal(botProperties.CurrentPhysicalHealth, scriptGlobals.CurrentPhysicalHealth);
            Assert.Equal(botProperties.MaximumStamina, scriptGlobals.MaximumStamina);
            Assert.Equal(botProperties.CurrentStamina, scriptGlobals.CurrentStamina);
        }

        [Theory]
        [ClassData(typeof(ScriptGlobalsSetsCurrentMoveTheoryData))]
        public void ScriptGlobals_Action_Correctly_Sets_CurrentMove_If_Idling(Action<ScriptGlobals> action, Moves expectedMove)
        {
            // Arrange
            var botProperties = BuildBotProperties();
            var scriptGlobals = new ScriptGlobals(botProperties);
            var originalMove = botProperties.CurrentMove;

            // Act
            action(scriptGlobals);

            // Assert
            Assert.Equal(Moves.Idling, originalMove);
            Assert.Equal(expectedMove, botProperties.CurrentMove);
        }

        [Theory]
        [ClassData(typeof(ScriptGlobalsIgnoresCurrentMoveTheoryData))]
        public void ScriptGlobals_Action_Correctly_Ignores_CurrentMove_If_Not_Idling(Action<ScriptGlobals> action, Moves originalMove)
        {
            // Arrange
            var botProperties = BuildBotProperties();
            botProperties.CurrentMove = originalMove;
            var scriptGlobals = new ScriptGlobals(botProperties);

            // Act
            action(scriptGlobals);

            // Assert
            Assert.NotEqual(Moves.Idling, originalMove);
            Assert.Equal(originalMove, botProperties.CurrentMove);
        }

        private BotProperties BuildBotProperties()
        {
            var bot = new BotDto
            {
                LocationX = 1,
                LocationY = 2,
                Orientation = Orientations.North,
                PreviousMove = Moves.Idling,
                MaximumHealth = 100,
                CurrentHealth = 99,
                MaximumStamina = 250,
                CurrentStamina = 150
            };
            var arena = new ArenaDto { Width = 10, Height = 20 };
            return BotProperties.Build(bot, arena);
        }

        private class ScriptGlobalsSetsCurrentMoveTheoryData : TheoryData<Action<ScriptGlobals>, Moves>
        {
            public ScriptGlobalsSetsCurrentMoveTheoryData()
            {
                Add(g => g.MoveForward(), Moves.WalkForward);
                Add(g => g.TurnLeft(), Moves.TurningLeft);
                Add(g => g.TurnRight(), Moves.TurningRight);
                Add(g => g.TurnAround(), Moves.TurningAround);
                Add(g => g.SelfDestruct(), Moves.SelfDestruct);
                Add(g => g.MeleeAttack(), Moves.MeleeAttack);
                Add(g => g.RangedAttack(0, 0), Moves.RangedAttack);
                Add(g => g.Teleport(0, 0), Moves.Teleport);
            }
        }

        private class ScriptGlobalsIgnoresCurrentMoveTheoryData : TheoryData<Action<ScriptGlobals>, Moves>
        {
            public ScriptGlobalsIgnoresCurrentMoveTheoryData()
            {
                Add(g => g.MoveForward(), Moves.MeleeAttack);
                Add(g => g.TurnLeft(), Moves.WalkForward);
                Add(g => g.TurnRight(), Moves.WalkForward);
                Add(g => g.TurnAround(), Moves.WalkForward);
                Add(g => g.SelfDestruct(), Moves.WalkForward);
                Add(g => g.MeleeAttack(), Moves.WalkForward);
                Add(g => g.RangedAttack(0, 0), Moves.WalkForward);
                Add(g => g.Teleport(0, 0), Moves.WalkForward);
            }
        }
    }
}