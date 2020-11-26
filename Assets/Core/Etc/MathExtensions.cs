using System;

namespace Core.Etc
{
    public static class MathExtensions
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
            => val.CompareTo(min) < 0 ? min : val.CompareTo(max) > 0 ? max : val;
    }
}