using CSharpWars.Common.Helpers;
using CSharpWars.Enums;
using CSharpWars.Tests.Framework.FluentAssertions;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Common.Helpers
{
    public class RandomHelperTest
    {
        [Fact]
        public void RandomHelper_Get_Returns_A_Random_Integer_Between_Zero_And_MaxValue()
        {
            // Arrange
            var helper = new RandomHelper();

            // Act
            var result = helper.Get(5);

            // Assert
            result.Should().BeOneOf(0, 1, 2, 3, 4);
        }


        [Fact]
        public void RandomHelper_Get_Returns_A_Random_Integer_Between_MinValue_And_MaxValue()
        {
            // Arrange
            var helper = new RandomHelper();

            // Act
            var result = helper.Get(5, 10);

            // Assert
            result.Should().BeOneOf(5, 6, 7, 8, 9);
        }


        [Fact]
        public void RandomHelper_Get_Returns_A_Random_Enum_Value()
        {
            // Arrange
            var helper = new RandomHelper();

            // Act
            var result = helper.Get<PossibleOrientations>();

            // Assert
            result.Should().BeOneOf(PossibleOrientations.North, PossibleOrientations.East, PossibleOrientations.South, PossibleOrientations.West);
        }
    }
}