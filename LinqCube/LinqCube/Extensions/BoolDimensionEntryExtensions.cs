using System;
using System.Collections.Generic;

namespace dasz.LinqCube
{
    /// <summary>
    /// Static helper class for building dimensions
    /// </summary>
    public static class BoolDimensionEntryExtensions
    {
        /// <summary>
        /// Builds a bool dimension.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<DimensionEntry<bool>> BuildBool(this DimensionEntry<bool> parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            return parent
                .AddBoolChild(false)
                .AddBoolChild(true)
                .Children;
        }

        private static DimensionEntry<bool> AddBoolChild(this DimensionEntry<bool> parent, bool value) =>
            parent.AddChild(value.ToStringInvariant(), value);
    }
}