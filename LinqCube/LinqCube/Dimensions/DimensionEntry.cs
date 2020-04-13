using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace dasz.LinqCube
{
    /// <summary>
    /// Implements a dimension entry
    /// </summary>
    /// <typeparam name="TDimension"></typeparam>
    public class DimensionEntry<TDimension> : IDimensionEntry
        where TDimension : IComparable
    {
        /// <summary>
        /// Name of the dimension entry.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Return the root dimension
        /// </summary>
        public virtual IDimension Root => Parent?.Root;

        /// <summary>
        /// Returns the parent dimension entry
        /// </summary>
        public IDimensionEntry Parent { get; private set; }

        /// <summary>
        /// Returns all children
        /// </summary>
        public List<DimensionEntry<TDimension>> Children { get; private set; }

        IEnumerable<IDimensionEntry> IDimensionEntry.Children => Children.Cast<IDimensionEntry>();

        /// <summary>
        /// Returns all children
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IDimensionEntry> GetEnumerator() =>
            Children.Cast<IDimensionEntry>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Min Value, incl.
        /// </summary>
        public virtual TDimension Min { get; set; }

        /// <summary>
        /// Max Value, excl.
        /// </summary>
        public virtual TDimension Max { get; set; }

        /// <summary>
        /// Signals, that the entry has a value
        /// </summary>
        private bool hasValue { get; set; }

        /// <summary>
        /// Distinct value
        /// </summary>
        private TDimension _value;
        public TDimension Value
        {
            get => _value;
            set
            {
                _value = value;
                hasValue = value == null ? false : true;
                if (Parent != null)
                    ((DimensionEntry<TDimension>)Parent).hasValue = hasValue;
            }
        }

        /// <summary>
        /// Creats a new dimension entry
        /// </summary>
        /// <param name="name"></param>
        internal DimensionEntry(string name)
            : this(name, null)
        {
        }

        /// <summary>
        /// Creats a new dimension entry
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        internal DimensionEntry(string name, DimensionEntry<TDimension> parent)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null, empty or whitespaces.", nameof(name));

            Name = name;
            Parent = parent;
            Children = new List<DimensionEntry<TDimension>>();
        }

        internal DimensionEntry<TDimension> AddChild(string name, TDimension value)
        {
            Children.Add(new DimensionEntry<TDimension>(name, this)
            {
                Value = value,
                Min = value,
                Max = value
            });

            return this;
        }

        internal DimensionEntry<TDimension> AddChild(string name, TDimension min, TDimension max)
        {
            Children.Add(new DimensionEntry<TDimension>(name, this)
            {
                Min = min,
                Max = max
            });

            return this;
        }

        /// <summary>
        /// checks if the given value is in range of this entry
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool InRange(TDimension value)
        {
            if (hasValue)
            {
                if (Root == this)
                    return true;
                if (value == null)
                    return false;

                return Value.CompareTo(value) == 0;
            }

            if (Min == null || Max == null)
                return false;
            if (value == null || value.CompareTo(default(TDimension)) == 0)
                return false;

            return Min.CompareTo(value) <= 0 && Max.CompareTo(value) >= 0;
        }

        /// <summary>
        /// checks if this entry is in the given range
        /// </summary>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        /// <image url="$(SolutionDir)Images\Period Relations.png" scale="1.0" />
        public bool InRange(TDimension lower, TDimension upper)
        {
            // Currently asValue is set only for bool and enum
            if (hasValue)
                throw new InvalidOperationException("Tried filtering a range on a discrete dimension.");

            if (lower == null)
                throw new ArgumentNullException(nameof(lower), "Lower boundary value cannot be null.");

            if (upper == null)
                throw new ArgumentNullException(nameof(upper), "Upper boundary value cannot be null.");

            // Got Note3 from link below, as databases might be dirt or inconsistent
            // https://stackoverflow.com/questions/325933/determine-whether-two-date-ranges-overlap
            return (lower.CompareTo(Min) > 0 ? lower : Min).CompareTo
                   (upper.CompareTo(Max) < 0 ? upper : Max) <= 0;
        }

        #region Previoulsy from Extensions (check later)
        #endregion

        /// <summary>
        /// Returns a debug output with a left padding
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public string DebugOut(int level)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}: {2} - {3}\n", string.Empty.PadLeft(level), Name, Min, Max);
            foreach (var child in Children)
            {
                sb.AppendLine(child.DebugOut(level + 1));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a string representation
        /// </summary>
        /// <returns>
        /// The string representation
        /// </returns>
        public override string ToString() =>
            $"DimensionEntry: {Name} {{Value:{Value},Min:{Min},Max:{Max}}}";
    }
}