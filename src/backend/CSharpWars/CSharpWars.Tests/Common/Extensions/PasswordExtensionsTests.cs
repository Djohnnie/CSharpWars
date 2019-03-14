using CSharpWars.Common.Extensions;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Common.Extensions
{
    public class PasswordExtensionsTests
    {
        [Fact]
        public void HashPassword_Should_Create_A_Random_Salt_And_Random_Hashed_Password()
        {
            // Arrange
            var password = "Pa$$w0rd";

            // Act
            var result1 = password.HashPassword();
            var result2 = password.HashPassword();

            // Assert
            result1.Salt.Should().NotBeNull();
            result1.Hashed.Should().NotBeNull();
            result2.Salt.Should().NotBeNull();
            result2.Hashed.Should().NotBeNull();
            result1.Salt.Should().NotBe(result2.Salt);
            result1.Hashed.Should().NotBe(result2.Hashed);
        }

        [Fact]
        public void HashPassword_With_Provided_Salt_Should_Create_A_Fixed_Hashed_Password()
        {
            // Arrange
            var password = "Pa$$w0rd";
            var salt = "08IhqtPCEwbCSXj6LzduXA==";

            // Act
            var result1 = password.HashPassword(salt);
            var result2 = password.HashPassword(salt);

            // Assert
            result1.Salt.Should().NotBeNull();
            result1.Hashed.Should().NotBeNull();
            result2.Salt.Should().NotBeNull();
            result2.Hashed.Should().NotBeNull();
            result1.Salt.Should().Be(salt);
            result2.Salt.Should().Be(salt);
            result1.Hashed.Should().Be(result2.Hashed);
        }
    }
}