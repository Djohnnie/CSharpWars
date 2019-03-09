using System.Threading.Tasks;
using CSharpWars.Common.Tools;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Common.Tools
{
    public class SimpleStopwatchTests
    {
        [Fact]
        public async Task Test()
        {
            // Arrange
            var simpleStopwatch = new SimpleStopwatch();

            // Act
            await Task.Delay(1000);

            // Assert
            simpleStopwatch.ElapsedMilliseconds.Should().BeGreaterThan(1000);
        }
    }
}