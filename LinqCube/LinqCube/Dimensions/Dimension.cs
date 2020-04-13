using System;
using System.Linq;

namespace dasz.LinqCube
{
    /// <summary>
    /// Dimension descriptor
    /// </summary>
    public class Dimension<TDimension, TFact> : DimensionEntry<TDimension>, IDimension
        where TDimension : IComparable
    {
        /// <summary>
        /// Selects a fact from the query
        /// </summary>
        private Func<TFact, TDimension> Selector;
        /// <summary>
        /// Selects a fact from the query for range dimensions
        /// </summary>
        private Func<TFact, TDimension> EndSelector;
        /// <summary>
        /// Applies a filter
        /// </summary>
        private Func<TFact, bool> Filter;

        /// <summary>
        /// Returns the root dimension of the current dimension
        /// </summary>
        public override IDimension Root => this;

        /// <summary>
        /// Creates a new dimension
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selector"></param>
        public Dimension(string name, Func<TFact, TDimension> selector)
            : this(name, selector, null, null)
        {
        }

        /// <summary>
        /// Creates a new dimension
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startSelector"></param>
        /// <param name="endSelector"></param>
        public Dimension(string name, Func<TFact, TDimension> startSelector, Func<TFact, TDimension> endSelector)
            : this(name, startSelector, endSelector, null)
        {
        }

        /// <summary>
        /// Creates a new dimension
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selector"></param>
        /// <param name="filter"></param>
        public Dimension(string name, Func<TFact, TDimension> selector, Func<TFact, bool> filter)
            : this(name, selector, null, filter)
        {
        }

        /// <summary>
        /// Creates a new dimension
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startSelector"></param>
        /// <param name="endSelector"></param>
        /// <param name="filter"></param>
        public Dimension(string name, Func<TFact, TDimension> startSelector, Func<TFact, TDimension> endSelector, Func<TFact, bool> filter)
            : base(name)
        {
            Selector = startSelector;
            EndSelector = endSelector;
            Filter = filter;
        }

        public bool CheckFilter(TFact item, IDimensionEntry entry)
        {
            var classEntry = ((DimensionEntry<TDimension>)entry);
            if (Filter == null || Filter(item))
            {
                if (EndSelector == null)
                    return classEntry.InRange(Selector(item));
                else
                    return classEntry.InRange(Selector(item), EndSelector(item));
            }

            return false;
        }

        /// <summary>
        /// Returns the lower boundary
        /// </summary>
        public override TDimension Min
        {
            get => Children.Count > 0 ? Children.First().Min : default;
            set => throw new InvalidOperationException("Cannot set lower boundary value on basic dimension.");
        }

        /// <summary>
        /// Returns the upper boundary
        /// </summary>
        public override TDimension Max
        {
            get => Children.Count > 0 ? Children.Last().Max : default;
            set => throw new InvalidOperationException("Cannot set upper boundary value on basic dimension.");
        }

        /// <summary>
        /// Returns a string representation
        /// </summary>
        /// <returns>
        /// The string representation
        /// </returns>
        public override string ToString() =>
            $"Dimension: {Name} {{Value:{Value},Min:{Min},Max:{Max}}}";
    }
}