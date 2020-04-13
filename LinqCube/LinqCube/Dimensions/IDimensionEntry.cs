using System.Collections.Generic;

namespace dasz.LinqCube
{
    /// <summary>
    /// An entry of a dimension
    /// </summary>
    public interface IDimensionEntry : IEnumerable<IDimensionEntry>
    {
        /// <summary>
        /// Returns the entry name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Return the root dimension.
        /// </summary>
        IDimension Root { get; }

        /// <summary>
        /// Returns the parent dimension entry.
        /// </summary>
        IDimensionEntry Parent { get; }

        /// <summary>
        /// Returns all children.
        /// </summary>
        IEnumerable<IDimensionEntry> Children { get; }
    }
}