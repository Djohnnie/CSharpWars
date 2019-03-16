using System;
using System.Collections.Generic;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Scripting
{
    public class VisionTests
    {
        [Fact]
        public void Vision_Build_Should_Build_Correctly()
        {
            // Arrange
            var arena = new ArenaDto { Width = 1, Height = 1 };
            var bot = new BotDto { Id = Guid.NewGuid() };
            var bots = new List<BotDto>(new[] { bot });
            var botProperties = BotProperties.Build(bot, arena, bots);

            // Act
            var result = Vision.Build(botProperties);

            // Assert
            result.Should().NotBeNull();
            result.Bots.Should().NotBeNull();
            result.Bots.Should().HaveCount(0);
            result.FriendlyBots.Should().NotBeNull();
            result.FriendlyBots.Should().HaveCount(0);
            result.EnemyBots.Should().NotBeNull();
            result.EnemyBots.Should().HaveCount(0);
        }

        [Theory]
        [InlineData(PossibleOrientations.North, 0, 0, "ME", "ME", 1, 1, 0)]
        [InlineData(PossibleOrientations.North, 0, 0, "ME", "THEY", 1, 0, 1)]
        [InlineData(PossibleOrientations.North, 1, 0, "ME", "ME", 1, 1, 0)]
        [InlineData(PossibleOrientations.North, 1, 0, "ME", "THEY", 1, 0, 1)]
        [InlineData(PossibleOrientations.North, 2, 0, "ME", "ME", 1, 1, 0)]
        [InlineData(PossibleOrientations.North, 2, 0, "ME", "THEY", 1, 0, 1)]
        [InlineData(PossibleOrientations.North, 0, 1, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.North, 0, 1, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.North, 2, 1, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.North, 2, 1, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.North, 0, 2, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.North, 0, 2, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.North, 1, 2, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.North, 1, 2, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.North, 2, 2, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.North, 2, 2, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.East, 0, 0, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.East, 0, 0, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.East, 1, 0, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.East, 1, 0, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.East, 2, 0, "ME", "ME", 1, 1, 0)]
        [InlineData(PossibleOrientations.East, 2, 0, "ME", "THEY", 1, 0, 1)]
        [InlineData(PossibleOrientations.East, 0, 1, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.East, 0, 1, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.East, 2, 1, "ME", "ME", 1, 1, 0)]
        [InlineData(PossibleOrientations.East, 2, 1, "ME", "THEY", 1, 0, 1)]
        [InlineData(PossibleOrientations.East, 0, 2, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.East, 0, 2, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.East, 1, 2, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.East, 1, 2, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.East, 2, 2, "ME", "ME", 1, 1, 0)]
        [InlineData(PossibleOrientations.East, 2, 2, "ME", "THEY", 1, 0, 1)]
        [InlineData(PossibleOrientations.South, 0, 0, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.South, 0, 0, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.South, 1, 0, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.South, 1, 0, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.South, 2, 0, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.South, 2, 0, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.South, 0, 1, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.South, 0, 1, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.South, 2, 1, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.South, 2, 1, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.South, 0, 2, "ME", "ME", 1, 1, 0)]
        [InlineData(PossibleOrientations.South, 0, 2, "ME", "THEY", 1, 0, 1)]
        [InlineData(PossibleOrientations.South, 1, 2, "ME", "ME", 1, 1, 0)]
        [InlineData(PossibleOrientations.South, 1, 2, "ME", "THEY", 1, 0, 1)]
        [InlineData(PossibleOrientations.South, 2, 2, "ME", "ME", 1, 1, 0)]
        [InlineData(PossibleOrientations.South, 2, 2, "ME", "THEY", 1, 0, 1)]
        [InlineData(PossibleOrientations.West, 0, 0, "ME", "ME", 1, 1, 0)]
        [InlineData(PossibleOrientations.West, 0, 0, "ME", "THEY", 1, 0, 1)]
        [InlineData(PossibleOrientations.West, 1, 0, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.West, 1, 0, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.West, 2, 0, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.West, 2, 0, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.West, 0, 1, "ME", "ME", 1, 1, 0)]
        [InlineData(PossibleOrientations.West, 0, 1, "ME", "THEY", 1, 0, 1)]
        [InlineData(PossibleOrientations.West, 2, 1, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.West, 2, 1, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.West, 0, 2, "ME", "ME", 1, 1, 0)]
        [InlineData(PossibleOrientations.West, 0, 2, "ME", "THEY", 1, 0, 1)]
        [InlineData(PossibleOrientations.West, 1, 2, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.West, 1, 2, "ME", "THEY", 0, 0, 0)]
        [InlineData(PossibleOrientations.West, 2, 2, "ME", "ME", 0, 0, 0)]
        [InlineData(PossibleOrientations.West, 2, 2, "ME", "THEY", 0, 0, 0)]
        public void Vision_Build_Should_Identify_Bots_Correctly(
            PossibleOrientations orientation, Int32 x, Int32 y, String playerName, String botName, Int32 expectedBots, Int32 expectedFriendlies, Int32 expectedEnemies)
        {
            // Arrange
            var arena = new ArenaDto { Width = 3, Height = 3 };
            var playerBot = new BotDto { Id = Guid.NewGuid(), X = 1, Y = 1, PlayerName = playerName, Orientation = orientation };
            var otherBot = new BotDto { Id = Guid.NewGuid(), X = x, Y = y, PlayerName = botName };
            var bots = new List<BotDto>(new[] { playerBot, otherBot });
            var botProperties = BotProperties.Build(playerBot, arena, bots);

            // Act
            var result = Vision.Build(botProperties);

            // Assert
            result.Should().NotBeNull();
            result.Bots.Should().NotBeNull();
            result.Bots.Should().HaveCount(expectedBots);
            result.FriendlyBots.Should().NotBeNull();
            result.FriendlyBots.Should().HaveCount(expectedFriendlies);
            result.EnemyBots.Should().NotBeNull();
            result.EnemyBots.Should().HaveCount(expectedEnemies);
        }
    }
}