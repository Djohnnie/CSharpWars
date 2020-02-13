using System;

namespace CSharpWars.Common.Helpers.Interfaces
{
    public interface IRandomHelper
    {
        int Get(int maxValue);

        int Get(int minValue, int maxValue);

        TEnum Get<TEnum>() where TEnum : Enum;
    }
}