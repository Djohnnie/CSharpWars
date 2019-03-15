using System;
using System.Collections.Generic;
using CSharpWars.Common.Extensions;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;
using CSharpWars.Tests.Framework.FluentAssertions;
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
            var scriptGlobals = ScriptGlobals.Build(botProperties);

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
        public void ScriptGlobals_Action_Correctly_Sets_CurrentMove_If_Idling(Action<ScriptGlobals> action, PossibleMoves expectedMove, Int32 moveDestinationX, Int32 moveDestinationY)
        {
            // Arrange
            var botProperties = BuildBotProperties();
            var scriptGlobals = ScriptGlobals.Build(botProperties);
            var originalMove = botProperties.CurrentMove;

            // Act
            action(scriptGlobals);

            // Assert
            originalMove.Should().Be(PossibleMoves.Idling);
            botProperties.CurrentMove.Should().Be(expectedMove);
            botProperties.MoveDestinationX.Should().Be(moveDestinationX);
            botProperties.MoveDestinationY.Should().Be(moveDestinationY);
        }

        [Theory]
        [ClassData(typeof(ScriptGlobalsIgnoresCurrentMoveTheoryData))]
        public void ScriptGlobals_Action_Correctly_Ignores_CurrentMove_If_Not_Idling(Action<ScriptGlobals> action, PossibleMoves originalMove)
        {
            // Arrange
            var botProperties = BuildBotProperties();
            botProperties.CurrentMove = originalMove;
            var scriptGlobals = ScriptGlobals.Build(botProperties);

            // Act
            action(scriptGlobals);

            // Assert
            originalMove.Should().NotBe(PossibleMoves.Idling);
            botProperties.CurrentMove.Should().Be(originalMove);
        }

        [Fact]
        public void ScriptGlobals_StoreInMemory_Should_Store_A_Value_In_Memory()
        {
            // Arrange
            var botProperties = BuildBotProperties();
            var scriptGlobals = ScriptGlobals.Build(botProperties);
            var key = "VAL";
            var value = 42;

            // Act
            scriptGlobals.StoreInMemory(key, value);

            // Assert
            botProperties.Memory.Should().HaveCount(1);
            botProperties.Memory.Should().ContainKey(key);
            botProperties.Memory.Should().ContainValue(value.Serialize());
        }

        [Fact]
        public void ScriptGlobals_StoreInMemory_Should_Edit_An_Existing_Value_In_Memory()
        {
            // Arrange
            var botProperties = BuildBotProperties();
            var scriptGlobals = ScriptGlobals.Build(botProperties);
            var key = "VAL";
            var value1 = 42;
            var value2 = 21;

            // Act
            scriptGlobals.StoreInMemory(key, value1);
            scriptGlobals.StoreInMemory(key, value2);

            // Assert
            botProperties.Memory.Should().HaveCount(1);
            botProperties.Memory.Should().ContainKey(key);
            botProperties.Memory.Should().ContainValue(value2.Serialize());
        }

        [Fact]
        public void ScriptGlobals_LoadFromMemory_Should_Return_A_Default_Value_For_Unknown_Key()
        {
            // Arrange
            var botProperties = BuildBotProperties();
            var scriptGlobals = ScriptGlobals.Build(botProperties);
            var key = "VAL";

            // Act
            var result = scriptGlobals.LoadFromMemory<Int32>(key);

            // Assert
            result.Should().BeZero();
        }

        [Fact]
        public void ScriptGlobals_LoadFromMemory_Should_Return_A_Stored_Value()
        {
            // Arrange
            var botProperties = BuildBotProperties();
            var scriptGlobals = ScriptGlobals.Build(botProperties);
            var key = "VAL";
            var value = 42;
            scriptGlobals.StoreInMemory(key, value);

            // Act
            var result = scriptGlobals.LoadFromMemory<Int32>(key);

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void ScriptGlobals_RemoveFromMemory_Should_Remove_A_Stored_Value()
        {
            // Arrange
            var botProperties = BuildBotProperties();
            var scriptGlobals = ScriptGlobals.Build(botProperties);
            var key = "VAL";
            var value = 42;
            scriptGlobals.StoreInMemory(key, value);

            // Act
            scriptGlobals.RemoveFromMemory(key);

            // Assert
            botProperties.Memory.Should().HaveCount(0);
        }

        [Fact]
        public void ScriptGlobals_Talk_Should_Add_Entry_To_Messages()
        {
            // Arrange
            var botProperties = BuildBotProperties();
            var scriptGlobals = ScriptGlobals.Build(botProperties);
            var message = "Message!";

            // Act
            scriptGlobals.Talk(message);

            // Assert
            botProperties.Messages.Should().HaveCount(1);
            botProperties.Messages.Should().Contain(message);
        }

        private BotProperties BuildBotProperties()
        {
            var bot = new BotDto
            {
                X = 1,
                Y = 2,
                Orientation = PossibleOrientations.North,
                Move = PossibleMoves.Idling,
                MaximumHealth = 100,
                CurrentHealth = 99,
                MaximumStamina = 250,
                CurrentStamina = 150,
                Memory = new Dictionary<String, String>().Serialize()
            };
            var arena = new ArenaDto { Width = 10, Height = 20 };
            return BotProperties.Build(bot, arena, new List<BotDto>());
        }

        private class ScriptGlobalsSetsCurrentMoveTheoryData : TheoryData<Action<ScriptGlobals>, PossibleMoves, Int32, Int32>
        {
            public ScriptGlobalsSetsCurrentMoveTheoryData()
            {
                Add(g => g.WalkForward(), PossibleMoves.WalkForward, 0, 0);
                Add(g => g.TurnLeft(), PossibleMoves.TurningLeft, 0, 0);
                Add(g => g.TurnRight(), PossibleMoves.TurningRight, 0, 0);
                Add(g => g.TurnAround(), PossibleMoves.TurningAround, 0, 0);
                Add(g => g.SelfDestruct(), PossibleMoves.SelfDestruct, 0, 0);
                Add(g => g.MeleeAttack(), PossibleMoves.MeleeAttack, 0, 0);
                Add(g => g.RangedAttack(2, 3), PossibleMoves.RangedAttack, 2, 3);
                Add(g => g.Teleport(2, 3), PossibleMoves.Teleport, 2, 3);
            }
        }

        private class ScriptGlobalsIgnoresCurrentMoveTheoryData : TheoryData<Action<ScriptGlobals>, PossibleMoves>
        {
            public ScriptGlobalsIgnoresCurrentMoveTheoryData()
            {
                Add(g => g.WalkForward(), PossibleMoves.MeleeAttack);
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