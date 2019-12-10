using System;
using System.Collections.Generic;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;
using CSharpWars.Processor.Middleware;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Scripting.Middleware
{
    public class ProcessingContextTests
    {
        [Fact]
        public void ProcessingContext_Build_Should_Build_A_Valid_ProcessingContext()
        {
            // Arrange
            var arena = new ArenaDto { Width = 4, Height = 5 };
            var bots = new List<BotDto>();

            // Act
            var result = ProcessingContext.Build(arena, bots);

            // Assert
            result.Should().NotBeNull();
            result.Arena.Should().BeSameAs(arena);
            result.Bots.Should().BeSameAs(bots);
        }

        [Fact]
        public void ProcessingContext_AddBotProperties_Should_Add_BotProperties()
        {
            // Arrange
            var arena = new ArenaDto { Width = 4, Height = 5 };
            var bot = new BotDto { Id = Guid.NewGuid() };
            var bots = new List<BotDto>(new[] { bot });
            var processingContext = ProcessingContext.Build(arena, bots);
            var botProperties = BotProperties.Build(bot, arena, bots);

            // Act
            processingContext.AddBotProperties(bot.Id, botProperties);
            var result = processingContext.GetBotProperties(bot.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(botProperties);
        }

        [Fact]
        public void ProcessingContext_GetOrderedBotProperties_Should_Order_BotProperties()
        {
            // Arrange
            var arena = new ArenaDto { Width = 4, Height = 5 };
            var bot1 = new BotDto { Id = Guid.NewGuid() };
            var bot2 = new BotDto { Id = Guid.NewGuid() };
            var bots = new List<BotDto>(new[] { bot1, bot2 });
            var processingContext = ProcessingContext.Build(arena, bots);
            var botProperties1 = BotProperties.Build(bot1, arena, bots);
            botProperties1.CurrentMove = PossibleMoves.WalkForward;
            var botProperties2 = BotProperties.Build(bot2, arena, bots);
            botProperties2.CurrentMove = PossibleMoves.MeleeAttack;
            processingContext.AddBotProperties(bot1.Id, botProperties1);
            processingContext.AddBotProperties(bot2.Id, botProperties2);

            // Act
            var result = processingContext.GetOrderedBotProperties();

            // Assert
            result.Should().NotBeNull();
            result.Should().ContainInOrder(botProperties2, botProperties1);
        }
    }
}