using System;

namespace CSharpWars.Tests.Framework.FluentAssertions
{
    public static class EnumExtensions
    {
        public static EnumAssertions<TEnum> Should<TEnum>(this TEnum value) where TEnum : Enum
        {
            return new EnumAssertions<TEnum>(value);
        }
    }
}