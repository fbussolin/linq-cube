using System;
using System.Collections.Generic;
using System.Linq;

namespace dasz.LinqCube
{
    public static class GenericDimensionEntryExtensions
    {
        /// <summary>
        /// Finally builds a dimension
        /// </summary>
        /// <typeparam name="TDimension"></typeparam>
        /// <typeparam name="TFact"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static Dimension<TDimension, TFact> Build<TDimension, TFact>(this List<DimensionEntry<TDimension>> lst)
            where TDimension : IComparable
        {
            return (Dimension<TDimension, TFact>)lst.First().Root;
        }

        /// <summary>
        /// Flattens a Dimensions hierarchie
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dim"></param>
        /// <returns></returns>
        public static IEnumerable<DimensionEntry<TDimension>> FlattenHierarchy<TDimension>(this DimensionEntry<TDimension> dim)
            where TDimension : IComparable
        {
            if (dim == null)
                throw new ArgumentNullException(nameof(dim));

            var result = new List<DimensionEntry<TDimension>>();

            foreach (DimensionEntry<TDimension> c in dim)
            {
                result.Add(c);
                result.AddRange(c.FlattenHierarchy());
            }
            return result;
        }

        internal static List<DimensionEntry<T>> BuildPartition<T>(
            this DimensionEntry<T> parent,
            T stepSize,
            T lowerLimit,
            T upperLimit,
            T minValue,
            T maxValue,
            string lowerLabelFormat,
            string defaultLabelFormat,
            string upperLabelFormat,
            Func<T, T, T> add)
            where T : IComparable
        {
            if (upperLimit.CompareTo(lowerLimit) <= 0)
                throw new ArgumentOutOfRangeException(nameof(upperLimit), $"{nameof(upperLimit)} must be greater than {nameof(lowerLimit)}.");

            if (stepSize.CompareTo(default(T)) <= 0)
                throw new ArgumentOutOfRangeException(nameof(stepSize), $"{nameof(stepSize)} must be greater than default.");

            var rangeFrom = minValue;
            var rangeTo = lowerLimit;

            while (rangeTo.CompareTo(upperLimit) <= 0)
            {
                if (rangeTo.CompareTo(lowerLimit) == 0)
                    parent.AddChildHelper(minValue, rangeTo, lowerLabelFormat);
                else
                    parent.AddChildHelper(rangeFrom, rangeTo, defaultLabelFormat);

                rangeFrom = rangeTo;
                rangeTo = add(rangeTo, stepSize);

                if (rangeTo.CompareTo(upperLimit) > 0)
                    parent.AddChildHelper(rangeFrom, maxValue, upperLabelFormat);
            }

            return parent.Children;
        }

        private static void AddChildHelper<T>(this DimensionEntry<T> parent, T from, T to, string labelFormat)
            where T : IComparable =>
            parent.AddChild(labelFormat.FormatInvariant(from, to), from, to);
    }
}