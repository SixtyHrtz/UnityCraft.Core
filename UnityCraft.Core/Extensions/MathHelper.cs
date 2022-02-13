using System;

namespace UnityCraft.Core.Extensions
{
    public static class MathHelper
    {
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            value = (value.CompareTo(max) > 0) ? max : value;
            value = (value.CompareTo(min) < 0) ? min : value;

            return value;
        }
    }
}
