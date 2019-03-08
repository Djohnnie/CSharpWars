using CSharpWars.Common.Helpers.Interfaces;
using System;

namespace CSharpWars.Common.Helpers
{
    public class RandomHelper : IRandomHelper
    {
        private readonly Random _randomGenerator = new Random();

        public Int32 Get(Int32 maxValue)
        {
            return _randomGenerator.Next(maxValue);
        }

        public Int32 Get(Int32 minValue, Int32 maxValue)
        {
            return _randomGenerator.Next(minValue, maxValue);
        }

        public TEnum Get<TEnum>() where TEnum : Enum
        {
            var values = Enum.GetValues(typeof(TEnum));
            var indexOfValue = _randomGenerator.Next(values.Length);
            return (TEnum)values.GetValue(indexOfValue);
        }
    }
}