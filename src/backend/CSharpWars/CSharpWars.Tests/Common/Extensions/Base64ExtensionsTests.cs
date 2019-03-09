using System;
using CSharpWars.Common.Extensions;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Common.Extensions
{
    public class Base64ExtensionsTests
    {
        [Fact]
        public void Base64Encode_Should_Encode_In_Base64()
        {
            // Arrange
            var input = "Text to encode!";
            var expectedOutput = "VGV4dCB0byBlbmNvZGUh";

            // Act
            var output = input.Base64Encode();

            // Assert
            output.Should().Be(expectedOutput);
        }

        [Fact]
        public void Base64Encode_Empty_String_Should_Return_Empty_String()
        {
            // Arrange
            var input = "";

            // Act
            var output = input.Base64Encode();

            // Assert
            output.Should().BeEmpty();
        }

        [Fact]
        public void Base64Encode_Null_String_Should_Return_Null_String()
        {
            // Arrange
            String input = null;

            // Act
            var output = input.Base64Encode();

            // Assert
            output.Should().BeNull();
        }

        [Fact]
        public void Base64Decode_Should_Decode_From_Base64()
        {
            // Arrange
            var input = "VGV4dCB0byBkZWNvZGUh";
            var expectedOutput = "Text to decode!";

            // Act
            var output = input.Base64Decode();

            // Assert
            output.Should().Be(expectedOutput);
        }

        [Fact]
        public void Base64Decode_Empty_String_Should_Return_Empty_String()
        {
            // Arrange
            var input = "";

            // Act
            var output = input.Base64Decode();

            // Assert
            output.Should().BeEmpty();
        }

        [Fact]
        public void Base64Decode_Null_String_Should_Return_Null_String()
        {
            // Arrange
            String input = null;

            // Act
            var output = input.Base64Decode();

            // Assert
            output.Should().BeNull();
        }
    }
}