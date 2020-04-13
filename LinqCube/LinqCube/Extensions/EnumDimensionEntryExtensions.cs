using System;
using System.Collections.Generic;
using System.Linq;

namespace dasz.LinqCube
{
    /// <summary>
    /// Static helper class for building dimensions
    /// </summary>
    public static class EnumDimensionEntryExtensions
    {
        /// <summary>
        /// Builds a simple string bases enumeration dimension
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="entries"></param>
        /// <returns></returns>
        public static List<DimensionEntry<string>> BuildEnum(this DimensionEntry<string> parent, params string[] entries)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            if (entries == null)
                throw new ArgumentNullException(nameof(entries));

            foreach (var entry in entries)
            {
                if (parent.Children.Any(x => x.Value == entry))
                    throw new ArgumentException("Entries should be unique.", nameof(entries));

                parent.AddChild(entry, entry);
            }

            return parent.Children;
        }

        /// <summary>
        /// Builds a enumeration dimension from the given Enum Type.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="parent"></param>
        /// <remarks>As enum is non-nullable type then min and max are set to the same as value</remarks>
        /// <returns></returns>
        public static List<DimensionEntry<TEnum>> BuildEnum<TEnum>(this DimensionEntry<TEnum> parent)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            if (!typeof(TEnum).IsEnum)
                throw new InvalidOperationException("TEnum is not an enumeration.");

            foreach (var value in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
                parent.AddChild(value.ToStringInvariant(), value);

            return parent.Children;
        }
    }
}