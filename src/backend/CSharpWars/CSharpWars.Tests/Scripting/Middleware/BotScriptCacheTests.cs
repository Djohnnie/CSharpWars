using System;
using CSharpWars.Processor.Middleware;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Xunit;

namespace CSharpWars.Tests.Scripting.Middleware
{
    public class BotScriptCacheTests
    {
        [Fact]
        public void BotScriptCache_ScriptStored_For_Unknown_BotId_Should_Return_False()
        {
            // Arrange
            var botScriptCache = new BotScriptCache();

            // Act
            var result = botScriptCache.ScriptStored(Guid.NewGuid());

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void BotScriptCache_ScriptStored_For_Known_BotId_Should_Return_True()
        {
            // Arrange
            var botScriptCache = new BotScriptCache();
            var botId = Guid.NewGuid();
            botScriptCache.StoreScript(botId, CSharpScript.Create(""));

            // Act
            var result = botScriptCache.ScriptStored(botId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void BotScriptCache_LoadScript_For_Known_BotId_Should_Return()
        {
            // Arrange
            var botScriptCache = new BotScriptCache();
            var botId = Guid.NewGuid();
            var script = CSharpScript.Create("");
            botScriptCache.StoreScript(botId, script);

            // Act
            var result = botScriptCache.LoadScript(botId);

            // Assert
            result.Should().Be(script);
        }

        [Fact]
        public void BotScriptCache_LoadScript_For_Known_BotId_Should_Throw_Exception()
        {
            // Arrange
            var botScriptCache = new BotScriptCache();

            // Act
            botScriptCache.Invoking(x => x.LoadScript(Guid.NewGuid()))

                // Assert
                .Should().Throw<ArgumentException>();
        }

        [Fact]
        public void BotScriptCache_ClearScript_For_Known_BotId_Should_Succeed()
        {
            // Arrange
            var botScriptCache = new BotScriptCache();
            var botId = Guid.NewGuid();
            var script = CSharpScript.Create("");
            botScriptCache.StoreScript(botId, script);

            // Act
            botScriptCache.ClearScript(botId);

            // Assert
            botScriptCache.ScriptStored(botId).Should().BeFalse();
        }
    }
}