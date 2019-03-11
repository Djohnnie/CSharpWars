using System.Threading.Tasks;
using CSharpWars.Common.Tools;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Common.Tools
{
    public class SimpleStopwatchTests
    {
        [Fact]
        public async Task SimpleStopwatch_Should_Start_On_Construction()
        {
            // Arrange
            var simpleStopwatch = new SimpleStopwatch();

            // Act
            await Task.Delay(1100);

            // Assert
            simpleStopwatch.ElapsedMilliseconds.Should().BeGreaterThan(1000);
        }

        [Fact]
        public async Task SimpleStopwatch_Should_Stop_On_Dispose()
        {
            // Arrange
            var simpleStopwatch = new SimpleStopwatch();

            // Act
            simpleStopwatch.Dispose();
            await Task.Delay(1100);

            // Assert
            simpleStopwatch.ElapsedMilliseconds.Should().BeLessThan(1000);
        }
    }
}