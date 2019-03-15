using System;
using System.IO;
using CSharpWars.Logging;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Logger
{
    public class ConsoleLoggerTests
    {
        [Fact]
        public void ConsoleLogger_Log_Should_Write_Message_To_Console_With_Additional_NewLine()
        {
            // Arrange
            var stringWriter = new StringWriter();
            var originalOutput = Console.Out;
            Console.SetOut(stringWriter);
            var consoleLogger = new ConsoleLogger();
            var message = "This is my message!";
            var expectedOutput = $"{message}{Environment.NewLine}";

            // Act
            consoleLogger.Log(message);

            // Assert
            stringWriter.ToString().Should().Be(expectedOutput);

            // Cleanup
            Console.SetOut(originalOutput);
        }
    }
}