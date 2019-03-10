using System;
using CSharpWars.Common.Extensions;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Common.Extensions
{
    public class SerializationExtensionsTests
    {
        public class Data
        {
            public Boolean BooleanProperty { get; set; }
            public String StringProperty { get; set; }
            public Int32 IntegerProperty { get; set; }
        }

        [Fact]
        public void SerializationExtensions_Should_Be_Able_To_Serialize()
        {
            // Arrange
            var input = new Data
            {
                BooleanProperty = true,
                StringProperty = "Hello world!",
                IntegerProperty = 42
            };
            var expectedOutput = "{\"BooleanProperty\":true,\"StringProperty\":\"Hello world!\",\"IntegerProperty\":42}";

            // Act
            var result = input.Serialize();

            // Assert
            result.Should().Be(expectedOutput);
        }

        [Fact]
        public void SerializationExtensions_Should_Be_Able_To_Serialize_Null_Object()
        {
            // Arrange
            Data input = null;
            var expectedOutput = String.Empty;

            // Act
            var result = input.Serialize();

            // Assert
            result.Should().Be(expectedOutput);
        }

        [Fact]
        public void SerializationExtensions_Should_Be_Able_To_Deserialize()
        {
            // Arrange
            var input = "{\"BooleanProperty\":true,\"StringProperty\":\"Hello world!\",\"IntegerProperty\":42}";
            var expectedOutput = new
            {
                BooleanProperty = true,
                StringProperty = "Hello world!",
                IntegerProperty = 42
            };

            // Act
            var result = input.Deserialize<Data>();

            // Assert
            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public void SerializationExtensions_Should_Be_Able_To_Deserialize_Empty_String()
        {
            // Arrange
            var input = "";

            // Act
            var result = input.Deserialize<Data>();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void SerializationExtensions_Should_Be_Able_To_Deserialize_Null_String()
        {
            // Arrange
            string input = null;

            // Act
            var result = input.Deserialize<Data>();

            // Assert
            result.Should().BeNull();
        }
    }
}