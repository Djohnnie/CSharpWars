using System;
using System.Collections.Generic;
using CSharpWars.Common.Extensions;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Scripting
{
    public class BotPropertiesTests
    {
        [Fact]
        public void BotProperties_Build_From_Bot_And_Arena_Should_Copy_Properties()
        {
            // Arrange
            var bot = new BotDto
            {
                Id = Guid.NewGuid(),
                X = 1,
                Y = 2,
                Orientation = PossibleOrientations.East,
                Move = PossibleMoves.Teleport,
                MaximumHealth = 100,
                CurrentHealth = 99,
                MaximumStamina = 1000,
                CurrentStamina = 999,
                Memory = new Dictionary<String, String>().Serialize()
            };
            var arena = new ArenaDto { Width = 7, Height = 8 };
            var bots = new List<BotDto>
            {
                new BotDto
                {
                    Id = Guid.NewGuid(), Name = "BotName", X = 22, Y = 33, Orientation = PossibleOrientations.West
                }
            };

            // Act
            var result = BotProperties.Build(bot, arena, bots);

            // Assert
            result.Should().NotBeNull();
            result.BotId.Should().Be(bot.Id);
            result.Width.Should().Be(arena.Width);
            result.Height.Should().Be(arena.Height);
            result.X.Should().Be(bot.X);
            result.Y.Should().Be(bot.Y);
            result.Orientation.Should().Be(bot.Orientation);
            result.LastMove.Should().Be(bot.Move);
            result.MaximumHealth.Should().Be(bot.MaximumHealth);
            result.CurrentHealth.Should().Be(bot.CurrentHealth);
            result.MaximumStamina.Should().Be(bot.MaximumStamina);
            result.CurrentStamina.Should().Be(bot.CurrentStamina);
            result.Memory.Should().BeEquivalentTo(bot.Memory.Deserialize<Dictionary<String, String>>());
            result.Messages.Should().BeEquivalentTo(new List<String>());
            result.Bots.Should().BeEquivalentTo(bots, p => p
                .Including(x => x.Id)
                .Including(x => x.Name)
                .Including(x => x.X)
                .Including(x => x.Y)
                .Including(x => x.Orientation));
            result.CurrentMove.Should().Be(PossibleMoves.Idling);
        }
    }
}