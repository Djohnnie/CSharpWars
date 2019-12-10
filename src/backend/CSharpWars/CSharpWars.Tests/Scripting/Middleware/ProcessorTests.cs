using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.DtoModel;
using CSharpWars.Processor.Middleware;
using CSharpWars.Processor.Middleware.Interfaces;
using Moq;
using Xunit;

namespace CSharpWars.Tests.Scripting.Middleware
{
    public class ProcessorTests
    {
        [Fact]
        public async Task Processor_Go_Should_Not_Call_Into_The_BotProcessingFactory_If_No_Bots_Are_Provided()
        {
            // Arrange
            var botProcessingFactory = new Mock<IBotProcessingFactory>();
            var processor = new Processor.Middleware.Processor(botProcessingFactory.Object);
            var arena = new ArenaDto { Width = 4, Height = 6 };
            var bots = new List<BotDto>();
            var processingContext = ProcessingContext.Build(arena, bots);

            // Act
            await processor.Go(processingContext);

            // Assert
            botProcessingFactory.Verify(x => x.Process(It.IsAny<BotDto>(), It.IsAny<ProcessingContext>()), Times.Never);
        }

        [Fact]
        public async Task Processor_Go_Should_Call_Into_The_BotProcessingFactory_For_Every_Bot_Provided()
        {
            // Arrange
            var botProcessingFactory = new Mock<IBotProcessingFactory>();
            var processor = new Processor.Middleware.Processor(botProcessingFactory.Object);
            var arena = new ArenaDto { Width = 4, Height = 6 };
            var bot1 = new BotDto { Id = Guid.NewGuid() };
            var bot2 = new BotDto { Id = Guid.NewGuid() };
            var bot3 = new BotDto { Id = Guid.NewGuid() };
            var bots = new List<BotDto>(new[] { bot1, bot2, bot3 });
            var processingContext = ProcessingContext.Build(arena, bots);

            // Act
            await processor.Go(processingContext);

            // Assert
            botProcessingFactory.Verify(x => x.Process(It.IsAny<BotDto>(), It.IsAny<ProcessingContext>()), Times.Exactly(3));
        }
    }
}