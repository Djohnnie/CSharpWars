using System;
using CSharpWars.Common.DependencyInjection;
using CSharpWars.Processor.DependencyInjection;
using CSharpWars.Processor.Middleware;
using CSharpWars.Processor.Middleware.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ProcessingMiddleware = CSharpWars.Processor.Middleware.Middleware;

namespace CSharpWars.Tests.Scripting.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        [Theory]
        [InlineData(typeof(IBotScriptCache), typeof(BotScriptCache))]
        [InlineData(typeof(IMiddleware), typeof(ProcessingMiddleware))]
        [InlineData(typeof(IPreprocessor), typeof(Preprocessor))]
        [InlineData(typeof(IProcessor), typeof(Processor.Middleware.Processor))]
        [InlineData(typeof(IPostprocessor), typeof(Postprocessor))]
        [InlineData(typeof(IBotProcessingFactory), typeof(BotProcessingFactory))]
        [InlineData(typeof(IBotScriptCompiler), typeof(BotScriptCompiler))]
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