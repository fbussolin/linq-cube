using System;
using System.Collections.Generic;
using System.Linq;

namespace dasz.LinqCube
{
    /// <summary>
    /// Static helper class for building dimensions
    /// </summary>
    public static class DecimalDimensionEntryExtensions
    {
        /// <summary>
        /// Builds a decimal partition dimension.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="stepSize"></param>
        /// <param name="lowerLimit"></param>
        /// <param name="upperLimit"></param>
        /// <param name="lowerLabelFormat"></param>
        /// <param name="defaultLabelFormat"></param>
        /// <param name="upperLabelFormat"></param>
        /// <returns></returns>
        public static List<DimensionEntry<decimal>> BuildPartition(
            this DimensionEntry<decimal> parent,
            decimal stepSize,
            decimal lowerLimit,
            decimal upperLimit,
            string lowerLabelFormat = "{0} - {1}",
            string defaultLabelFormat = "{0} - {1}",
            string upperLabelFormat = "{0} - {1}")
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            return parent.BuildPartition(
                stepSize,
                lowerLimit,
                upperLimit,
                decimal.MinValue,
                decimal.MaxValue,
                lowerLabelFormat,
                defaultLabelFormat,
                upperLabelFormat,
                (rangeTo, stepSize) => rangeTo + stepSize);
        }

        /// <summary>
        /// Builds a decimal partition dimension.
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="stepSize"></param>
        /// <returns></returns>
        public static List<DimensionEntry<decimal>> BuildPartition(
            this List<DimensionEntry<decimal>> lst,
            decimal stepSize)
        {
            if (lst == null)
                throw new ArgumentNullException(nameof(lst));

            foreach (var parent in lst)
            {
                if (parent.Min == decimal.MinValue || parent.Max == decimal.MaxValue)
                    parent.AddChild(parent.Name, parent.Min, parent.Max);
                else
                    parent.BuildPartition(
                        stepSize,
                        parent.Min + stepSize,
                        parent.Max - stepSize,
                        parent.Min,
                        parent.Max,
                        "{0} - {1}",
                        "{0} - {1}",
                        "{0} - {1}",
                        (rangeTo, stepSize) => rangeTo + stepSize);
            }

            return lst.SelectMany(i => i.Children).ToList();
        }
    }
}