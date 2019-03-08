using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;

namespace CSharpWars.Tests.Framework.FluentAssertions
{
    public class EnumAssertions<TEnum> where TEnum : Enum
    {
        public IComparable Subject { get; private set; }

        public EnumAssertions(TEnum value)
        {
            Subject = value;
        }

        public AndConstraint<EnumAssertions<TEnum>> Be(TEnum expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null) && Subject.CompareTo(expected) == 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to be {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<EnumAssertions<TEnum>>(this);
        }

        public AndConstraint<EnumAssertions<TEnum>> NotBe(TEnum unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject is null || Subject.CompareTo(unexpected) != 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect {context:value} to be {0}{reason}.", unexpected);

            return new AndConstraint<EnumAssertions<TEnum>>(this);
        }

        public AndConstraint<EnumAssertions<TEnum>> BeOneOf(params TEnum[] validValues)
        {
            return BeOneOf(validValues, string.Empty);
        }

        public AndConstraint<EnumAssertions<TEnum>> BeOneOf(IEnumerable<TEnum> validValues, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject is null) && validValues.Contains((TEnum)Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to be one of {0}{reason}, but found {1}.", validValues, Subject);

            return new AndConstraint<EnumAssertions<TEnum>>(this);
        }
    }
}