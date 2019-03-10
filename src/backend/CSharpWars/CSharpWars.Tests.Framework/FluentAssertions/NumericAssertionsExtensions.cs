using FluentAssertions;
using FluentAssertions.Numeric;

namespace CSharpWars.Tests.Framework.FluentAssertions
{
    public static class NumericAssertionsExtensions
    {
        public static AndConstraint<NumericAssertions<T>> BeZero<T>(this NumericAssertions<T> value) where T : struct
        {
            return value.Be(default(T));
        }
    }
}