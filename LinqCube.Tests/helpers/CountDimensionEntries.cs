using System;
using System.Collections.Generic;

namespace dasz.LinqCube.Tests
{
    public static class CountDimensionEntries
    {
        public static int CountChildrenContaining<T>(this IList<DimensionEntry<T>> entries, T value, T min, T max)
            where T : IComparable
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));

            int result = 0;

            foreach (var entry in entries)
            {
                if (NullOrCompareTo(entry.Value, value) &&
                    NullOrCompareTo(entry.Min, min) &&
                    NullOrCompareTo(entry.Max, max))
                    result++;
            }

            return result;
        }

        public static int CountChildrenContaining<T>(this IList<DimensionEntry<T>> entries, T value)
            where T : IComparable
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));

            int result = 0;

            foreach (var entry in entries)
            {
                if (NullOrCompareTo(entry.Value, value) &&
                    NullOrCompareTo(entry.Min, value) &&
                    NullOrCompareTo(entry.Max, value))
                    result++;
            }

            return result;
        }

        private static bool NullOrCompareTo<T>(T left, object right)
            where T : IComparable =>
            left == null ? right == null : left.CompareTo(right) == 0;
    }
}