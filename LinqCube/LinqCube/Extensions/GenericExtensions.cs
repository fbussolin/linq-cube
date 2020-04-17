using System;
using System.Globalization;

namespace dasz.LinqCube
{
    internal static class GenericExtensions
    {
        internal static string FormatInvariant(this string format, params object[] args)
            => string.Format(CultureInfo.InvariantCulture, format, args);

        internal static string ToStringInvariant(this int value) =>
            value.ToString(CultureInfo.InvariantCulture);

        internal static string ToStringInvariant(this bool value) =>
            value.ToString(CultureInfo.InvariantCulture);

        internal static string ToStringInvariant<TEnum>(this TEnum value)
            where TEnum : struct, IComparable, IConvertible, IFormattable =>
            value.ToString(CultureInfo.InvariantCulture);
    }
}