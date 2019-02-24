using System;
using System.Collections.Generic;
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
        public void ScriptGlobals_Action_Correctly_Sets_CurrentMove_If_Idling(Action<ScriptGlobals> action, Move expectedMove)
        {
            // Arrange
            var botProperties = BuildBotProperties();
            var scriptGlobals = new ScriptGlobals(botProperties);
            var originalMove = botProperties.CurrentMove;

            // Act
            action(scriptGlobals);

            // Assert
            Assert.Equal(Move.Idling, originalMove);
            Assert.Equal(expectedMove, botProperties.CurrentMove);
        }

        [Theory]
        [ClassData(typeof(ScriptGlobalsIgnoresCurrentMoveTheoryData))]
        public void ScriptGlobals_Action_Correctly_Ignores_CurrentMove_If_Not_Idling(Action<ScriptGlobals> action, Move originalMove)
        {
            // Arrange
            var botProperties = BuildBotProperties();
            botProperties.CurrentMove = originalMove;
            var scriptGlobals = new ScriptGlobals(botProperties);

            // Act
            action(scriptGlobals);

            // Assert
            Assert.NotEqual(Move.Idling, originalMove);
            Assert.Equal(originalMove, botProperties.CurrentMove);
        }

        private BotProperties BuildBotProperties()
        {
            return new BotProperties(
                10,
                20,
                1,
                2,
                Orientation.North,
                Move.Idling,
                100,
                99,
                250,
                150,
                new Dictionary<string, string>(),
                new List<string>());
        }

        private class ScriptGlobalsSetsCurrentMoveTheoryData : TheoryData<Action<ScriptGlobals>, Move>
        {
            public ScriptGlobalsSetsCurrentMoveTheoryData()
            {
                Add(g => g.MoveForward(), Move.MovingForward);
                Add(g => g.TurnLeft(), Move.TurningLeft);
                Add(g => g.TurnRight(), Move.TurningRight);
                Add(g => g.TurnAround(), Move.TurningAround);
                Add(g => g.SelfDestruct(), Move.SelfDestruct);
                Add(g => g.MeleeAttack(), Move.MeleeAttack);
                Add(g => g.RangedAttack(0, 0), Move.RangedAttack);
                Add(g => g.Teleport(0, 0), Move.Teleport);
            }
        }

        private class ScriptGlobalsIgnoresCurrentMoveTheoryData : TheoryData<Action<ScriptGlobals>, Move>
        {
            public ScriptGlobalsIgnoresCurrentMoveTheoryData()
            {
                Add(g => g.MoveForward(), Move.MeleeAttack);
                Add(g => g.TurnLeft(), Move.MovingForward);
                Add(g => g.TurnRight(), Move.MovingForward);
                Add(g => g.TurnAround(), Move.MovingForward);
                Add(g => g.SelfDestruct(), Move.MovingForward);
                Add(g => g.MeleeAttack(), Move.MovingForward);
                Add(g => g.RangedAttack(0, 0), Move.MovingForward);
                Add(g => g.Teleport(0, 0), Move.MovingForward);
            }
        }
    }
}