﻿using CSharpWars.Processor.Middleware;

namespace CSharpWars.Tests.Scripting.Middleware;

public class BotScriptCompilerTests
{
    [Fact]
    public void BotScriptCompiler_Compile()
    {
        // Arrange
        var botScriptCompiler = new BotScriptCompiler();
        var script = "";

        // Act
        var result = botScriptCompiler.Compile(script);

        // Assert
        result.Should().NotBeNull();
    }
}