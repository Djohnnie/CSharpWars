using System.Threading.Tasks;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Processor.Middleware;
using CSharpWars.Processor.Middleware.Interfaces;
using Moq;
using Xunit;
using MiddlewareProcessor = CSharpWars.Processor.Middleware.Middleware;

namespace CSharpWars.Tests.Scripting.Middleware
{
    public class MiddlewareTests
    {
        [Fact]
        public async Task Middleware_Process_Should_Call_All_Processing_Steps()
        {
            // Arrange
            var arenaLogic = new Mock<IArenaLogic>();
            var botLogic = new Mock<IBotLogic>();
            var messageLogic = new Mock<IMessageLogic>();
            var preprocessor = new Mock<IPreprocessor>();
            var processor = new Mock<IProcessor>();
            var postprocessor = new Mock<IPostprocessor>();
            var middleware = new MiddlewareProcessor(
                arenaLogic.Object, botLogic.Object, messageLogic.Object,
                preprocessor.Object, processor.Object, postprocessor.Object);

            // Mock

            // Act
            await middleware.Process();

            // Assert
            preprocessor.Verify(x => x.Go(It.IsAny<ProcessingContext>()), Times.Once);
            processor.Verify(x => x.Go(It.IsAny<ProcessingContext>()), Times.Once);
            postprocessor.Verify(x => x.Go(It.IsAny<ProcessingContext>()), Times.Once);
        }
    }
}