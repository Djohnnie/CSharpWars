using CSharpWars.Enums;
using CSharpWars.Processor.Middleware;
using CSharpWars.Tests.Framework.FluentAssertions;
using FluentAssertions;
using Xunit;

namespace CSharpWars.Tests.Scripting.Middleware
{
    public class MoveComparerTests
    {
        [Theory]
        [InlineData(PossibleMoves.Idling)]
        [InlineData(PossibleMoves.TurningLeft)]
        [InlineData(PossibleMoves.TurningRight)]
        [InlineData(PossibleMoves.TurningAround)]
        [InlineData(PossibleMoves.WalkForward)]
        [InlineData(PossibleMoves.Teleport)]
        [InlineData(PossibleMoves.MeleeAttack)]
        [InlineData(PossibleMoves.RangedAttack)]
        [InlineData(PossibleMoves.SelfDestruct)]
        [InlineData(PossibleMoves.Died)]
        [InlineData(PossibleMoves.ScriptError)]
        public void MoveComparer_Compare_Identical_PossibleMoves_Should_Return_Zero(PossibleMoves move)
        {
            // Arrange
            var moveComparer = new MoveComparer();

            // Act
            var result = moveComparer.Compare(move, move);

            // Assert
            result.Should().BeZero();
        }

        [Theory]
        [InlineData(PossibleMoves.TurningLeft, PossibleMoves.WalkForward)]
        [InlineData(PossibleMoves.TurningRight, PossibleMoves.WalkForward)]
        [InlineData(PossibleMoves.TurningAround, PossibleMoves.WalkForward)]
        public void MoveComparer_Compare_Turning_To_Walking_Should_Return_Negative(PossibleMoves turning, PossibleMoves walking)
        {
            // Arrange
            var moveComparer = new MoveComparer();

            // Act
            var result = moveComparer.Compare(turning, walking);

            // Assert
            result.Should().BePositive();
        }

        [Theory]
        [InlineData(PossibleMoves.WalkForward, PossibleMoves.Teleport)]
        public void MoveComparer_Compare_Walking_To_Teleporting_Should_Return_Negative(PossibleMoves walking, PossibleMoves teleporting)
        {
            // Arrange
            var moveComparer = new MoveComparer();

            // Act
            var result = moveComparer.Compare(walking, teleporting);

            // Assert
            result.Should().BePositive();
        }

        [Theory]
        [InlineData(PossibleMoves.Teleport, PossibleMoves.SelfDestruct)]
        public void MoveComparer_Compare_Teleporting_To_SelfDestructing_Should_Return_Negative(PossibleMoves teleporting, PossibleMoves selfDestructing)
        {
            // Arrange
            var moveComparer = new MoveComparer();

            // Act
            var result = moveComparer.Compare(teleporting, selfDestructing);

            // Assert
            result.Should().BePositive();
        }

        [Theory]
        [InlineData(PossibleMoves.SelfDestruct, PossibleMoves.MeleeAttack)]
        public void MoveComparer_Compare_SelfDestructing_To_MeleeAttacking_Should_Return_Negative(PossibleMoves selfDestructing, PossibleMoves meleeAttacking)
        {
            // Arrange
            var moveComparer = new MoveComparer();

            // Act
            var result = moveComparer.Compare(selfDestructing, meleeAttacking);

            // Assert
            result.Should().BePositive();
        }

        [Theory]
        [InlineData(PossibleMoves.MeleeAttack, PossibleMoves.RangedAttack)]
        public void MoveComparer_Compare_MeleeAttack_To_RangedAttack_Should_Return_Negative(PossibleMoves meleeAttack, PossibleMoves rangedAttack)
        {
            // Arrange
            var moveComparer = new MoveComparer();

            // Act
            var result = moveComparer.Compare(meleeAttack, rangedAttack);

            // Assert
            result.Should().BePositive();
        }
    }
}