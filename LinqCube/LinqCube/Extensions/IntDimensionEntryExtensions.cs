using System;
using System.Collections.Generic;
using System.Linq;

namespace dasz.LinqCube
{
    /// <summary>
    /// Static helper class for building dimensions
    /// </summary>
    public static class IntDimensionEntryExtensions
    {
        /// <summary>
        /// Builds an int partition dimension.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="stepSize"></param>
        /// <param name="lowerLimit"></param>
        /// <param name="upperLimit"></param>
        /// <param name="lowerLabelFormat"></param>
        /// <param name="defaultLabelFormat"></param>
        /// <param name="upperLabelFormat"></param>
        /// <returns></returns>
        public static List<DimensionEntry<int>> BuildPartition(
            this DimensionEntry<int> parent,
            int stepSize,
            int lowerLimit,
            int upperLimit,
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
                int.MinValue,
                int.MaxValue,
                lowerLabelFormat,
                defaultLabelFormat,
                upperLabelFormat,
                (rangeTo, stepSize) => rangeTo + stepSize);
        }
        /// <summary>
        /// Builds an int partition dimension.
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="stepSize"></param>
        /// <returns></returns>
        public static List<DimensionEntry<int>> BuildPartition(
            this List<DimensionEntry<int>> lst,
            int stepSize)
        {
            if (lst == null)
                throw new ArgumentNullException(nameof(lst));

            foreach (var parent in lst)
            {
                if (parent.Min == int.MinValue || parent.Max == int.MaxValue)
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