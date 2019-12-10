using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.DtoModel;
using CSharpWars.Processor.Middleware;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Scripting.Middleware
{
    public class PreprocessorTests
    {
        [Fact]
        public async Task Preprocessor_Go_Should_Not_Add_BotProperties_If_No_Bots_Provided()
        {
            // Arrange
            var arena = new ArenaDto { Width = 4, Height = 6 };
            var bots = new List<BotDto>();
            var processingContext = ProcessingContext.Build(arena, bots);
            var preprocessor = new Preprocessor();

            // Act
            await preprocessor.Go(processingContext);

            // Assert
            processingContext.GetOrderedBotProperties().Should().HaveCount(0);
        }

        [Fact]
        public async Task Preprocessor_Go_Should_Add_BotProperties_For_Each_Bot()
        {
            // Arrange
            var arena = new ArenaDto { Width = 4, Height = 6 };
            var bot = new BotDto { Id = Guid.NewGuid() };
            var bots = new List<BotDto>(new[] { bot });
            var processingContext = ProcessingContext.Build(arena, bots);
            var preprocessor = new Preprocessor();

            // Act
            await preprocessor.Go(processingContext);

            // Assert
            var botProperties = processingContext.GetBotProperties(bot.Id);
            botProperties.Should().NotBeNull();
            botProperties.BotId.Should().Be(bot.Id);
            processingContext.GetOrderedBotProperties().Should().HaveCount(1);
        }
    }
}