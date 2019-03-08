using System;

namespace CSharpWars.Common.Helpers.Interfaces
{
    public interface IRandomHelper
    {
        Int32 Get(Int32 maxValue);

        Int32 Get(Int32 minValue, Int32 maxValue);

        TEnum Get<TEnum>() where TEnum : Enum;
    }
}