using System;
using CSharpWars.Common.DependencyInjection;
using CSharpWars.ScriptProcessor;
using CSharpWars.ScriptProcessor.DependencyInjection;
using CSharpWars.ScriptProcessor.Interfaces;
using CSharpWars.ScriptProcessor.Middleware;
using CSharpWars.ScriptProcessor.Middleware.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CSharpWars.Tests.Scripting.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        [Theory]
        [InlineData(typeof(IBotScriptCache), typeof(BotScriptCache))]
        [InlineData(typeof(IPreprocessor), typeof(Preprocessor))]
        [InlineData(typeof(IProcessor), typeof(Processor))]
        [InlineData(typeof(IPostprocessor), typeof(Postprocessor))]
        public void ConfigureScriptProcessor_Should_Register_Singleton_Classes(Type type, Type implementation)
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.ConfigureScriptProcessor();
            serviceCollection.ConfigurationHelper(c =>
            {
                c.ArenaSize = 10;
                c.ConnectionString = "";
            });
            var provider = serviceCollection.BuildServiceProvider();
            var value1 = provider.GetService(type);
            var value2 = provider.GetService(type);

            // Assert
            value1.Should().BeOfType(implementation);
            value2.Should().BeOfType(implementation);
            value1.Should().Be(value2);
        }
    }
}