using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;
using CSharpWars.ScriptProcessor.Middleware;
using FluentAssertions;
using Moq;
using Xunit;

namespace CSharpWars.Tests.Scripting.Middleware
{
    public class PostprocessorTests
    {
        [Fact]
        public async Task Postprocessor_Go_Should_Work()
        {
            // Arrange
            var randomHelper = new Mock<IRandomHelper>();
            var postprocessor = new Postprocessor(randomHelper.Object);
            var arena = new ArenaDto { Width = 1, Height = 3 };
            var bot = new BotDto { Id = Guid.NewGuid(), X = 0, Y = 2, Orientation = PossibleOrientations.North, CurrentStamina = 10 };
            var bots = new List<BotDto>(new[] { bot });
            var context = ProcessingContext.Build(arena, bots);
            var botProperties = BotProperties.Build(bot, arena, bots);
            botProperties.CurrentMove = PossibleMoves.WalkForward;
            context.AddBotProperties(bot.Id, botProperties);

            // Act
            await postprocessor.Go(context);

            // Assert
            context.Bots.Should().HaveCount(1);
            context.Bots.Single().X.Should().Be(0);
            context.Bots.Single().Y.Should().Be(1);
            context.Bots.Single().CurrentStamina.Should().Be(9);
        }
    }
}