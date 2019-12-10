using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.Common.Extensions;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Logging.Interfaces;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Scripting.Model;
using CSharpWars.Processor.Middleware;
using CSharpWars.Processor.Middleware.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CSharpWars.Tests.Scripting.Middleware
{
    public class BotProcessingFactoryTests
    {
        [Fact]
        public async Task BotProcessingFactory_Process_Without_Valid_Script_Should_Register_ScriptError()
        {
            // Arrange
            var botLogic = new Mock<IBotLogic>();
            var botScriptCompiler = new Mock<IBotScriptCompiler>();
            var botScriptCache = new Mock<IBotScriptCache>();
            var logger = new Mock<ILogger>();
            var botProcessingFactory = new BotProcessingFactory(
                botLogic.Object, botScriptCompiler.Object, botScriptCache.Object, logger.Object);
            var arena = new ArenaDto { Width = 4, Height = 3 };
            var bot = new BotDto { Id = Guid.NewGuid() };
            var bots = new List<BotDto>(new[] { bot });
            var context = ProcessingContext.Build(arena, bots);
            context.AddBotProperties(bot.Id, BotProperties.Build(bot, arena, bots));

            // Act
            await botProcessingFactory.Process(bot, context);

            // Assert
            context.GetBotProperties(bot.Id).CurrentMove.Should().Be(PossibleMoves.ScriptError);
        }

        [Fact]
        public async Task BotProcessingFactory_Process_With_Valid_Script_Should_Register_Correct_Move()
        {
            // Arrange
            var botLogic = new Mock<IBotLogic>();
            var botScriptCompiler = new BotScriptCompiler();
            var botScriptCache = new BotScriptCache();
            var logger = new Mock<ILogger>();
            var botProcessingFactory = new BotProcessingFactory(
                botLogic.Object, botScriptCompiler, botScriptCache, logger.Object);
            var arena = new ArenaDto { Width = 4, Height = 3 };
            var bot = new BotDto { Id = Guid.NewGuid() };
            var bots = new List<BotDto>(new[] { bot });
            var context = ProcessingContext.Build(arena, bots);
            context.AddBotProperties(bot.Id, BotProperties.Build(bot, arena, bots));
            var botScript = "WalkForward();".Base64Encode();

            // Mock
            botLogic.Setup(x => x.GetBotScript(It.IsAny<Guid>())).ReturnsAsync(botScript);

            // Act
            await botProcessingFactory.Process(bot, context);

            // Assert
            context.GetBotProperties(bot.Id).CurrentMove.Should().Be(PossibleMoves.WalkForward);
        }
    }
}