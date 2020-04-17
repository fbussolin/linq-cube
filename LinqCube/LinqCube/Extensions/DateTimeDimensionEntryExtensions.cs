using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace dasz.LinqCube
{
    /// <summary>
    /// Static helper class for building dimensions
    /// </summary>
    public static class DateTimeDimensionEntryExtensions
    {
        /// <summary>
        /// Builds a dimension representing Years
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="fromYear"></param>
        /// <param name="thruYear"></param>
        /// <returns></returns>
        public static List<DimensionEntry<DateTime>> BuildYear(this DimensionEntry<DateTime> parent, int fromYear, int thruYear)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            for (int year = fromYear; year <= thruYear; year++)
                parent.AddChild(year.ToStringInvariant(),
                    new DateTime(year, 1, 1),
                    new DateTime(year, 12, 31));

            return parent.Children;
        }

        /// <summary>
        /// Build years from the given time dimensions
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static List<DimensionEntry<DateTime>> BuildYear(this List<DimensionEntry<DateTime>> lst)
        {
            if (lst == null)
                throw new ArgumentNullException(nameof(lst));

            foreach (var parent in lst)
                BuildYear(parent, parent.Min.Year, parent.Max.Year);

            return lst.SelectMany(i => i.Children).ToList();
        }

        /// <summary>
        /// Builds a dimension representing all Years in the the given range
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="from"></param>
        /// <param name="thruDay"></param>
        /// <returns></returns>
        public static List<DimensionEntry<DateTime>> BuildYearRange(this DimensionEntry<DateTime> parent, DateTime from, DateTime thruDay)
        {
            if (from != from.Date) throw
                    new ArgumentOutOfRangeException(nameof(from), "contains time component");

            if (thruDay != thruDay.Date)
                throw new ArgumentOutOfRangeException(nameof(thruDay), "contains time component");

            var children = BuildYear(parent, from.Year, thruDay.Year);
            children.First().Min = from;
            children.Last().Max = thruDay.AddDays(1);

            return parent.Children;
        }

        /// <summary>
        /// Builds a dimension representing Years in the given range. This method limits the years individual ends, e.g. 1.1. - 1.3. 
        /// This makes part of years comparable.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="fromYear"></param>
        /// <param name="thruYear"></param>
        /// <param name="sliceFromMonth"></param>
        /// <param name="sliceFromDay"></param>
        /// <param name="sliceThruMonth"></param>
        /// <param name="sliceThruDay"></param>
        /// <returns></returns>
        public static List<DimensionEntry<DateTime>> BuildYearSlice(this DimensionEntry<DateTime> parent, int fromYear, int thruYear, int sliceFromMonth = 1, int? sliceFromDay = null, int sliceThruMonth = 1, int? sliceThruDay = null)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            for (int year = fromYear; year <= thruYear; year++)
                parent.AddChild(year.ToStringInvariant(),
                    new DateTime(year, sliceFromMonth, sliceFromDay ?? 1),
                    new DateTime(year, sliceThruMonth, sliceThruDay ?? DateTime.DaysInMonth(year, sliceThruMonth)));

            return parent.Children;
        }

        /// <summary>
        /// Build quaters from the given time dimensions
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static List<DimensionEntry<DateTime>> BuildQuarter(this List<DimensionEntry<DateTime>> lst)
        {
            if (lst == null)
                throw new ArgumentNullException(nameof(lst));

            foreach (var parent in lst)
            {
                for (int quarter = 1; quarter <= 4; quarter++)
                {
                    var dtFrom = new DateTime(parent.Min.Year, ((quarter - 1) * 3) + 1, 1);
                    var dtUntil = dtFrom.AddMonths(3);
                    if (dtFrom < parent.Min) dtFrom = parent.Min;
                    if (dtUntil > parent.Max) dtUntil = parent.Max;

                    parent.Children.Add(new DimensionEntry<DateTime>(quarter.ToStringInvariant(), parent)
                    {
                        Min = dtFrom,
                        Max = dtUntil
                    });
                }
            }

            return lst.SelectMany(i => i.Children).ToList();
        }

        /// <summary>
        /// Build months from the given time dimensions
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static List<DimensionEntry<DateTime>> BuildMonths(
            this List<DimensionEntry<DateTime>> lst,
            string nameFormat = "MM",
            CultureInfo culture = null)
        {
            if (lst == null)
                throw new ArgumentNullException(nameof(lst));

            if (culture == null)
                culture = CultureInfo.InvariantCulture;

            foreach (var parent in lst)
            {
                for (var month = new DateTime(parent.Min.Year, parent.Min.Month, 1); month <= parent.Max; month = month.AddMonths(1))
                {
                    var dtFrom = month < parent.Min ? parent.Min : month;

                    var dtUntil = dtFrom.AddMonths(1).AddDays(-1);
                    if (dtUntil > parent.Max)
                        dtUntil = parent.Max;

                    if (dtUntil != dtFrom)
                        parent.AddChild(month.ToString(nameFormat, culture), dtFrom, dtUntil);
                }
            }

            return lst.SelectMany(i => i.Children).ToList();
        }

        /// <summary>
        /// Build weeks from the given time dimensions
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static List<DimensionEntry<DateTime>> BuildWeeks(this List<DimensionEntry<DateTime>> lst)
        {
            if (lst == null)
                throw new ArgumentNullException(nameof(lst));

            foreach (var parent in lst)
            {
                for (DateTime week = parent.Min.Date.AddDays(-(((int)parent.Min.DayOfWeek - 1) % 7)).AddDays(-7); week <= parent.Max; week = week.AddDays(7))
                {
                    var dtFrom = week < parent.Min ? parent.Min : week;

                    var dtUntil = dtFrom.AddDays(7);
                    if (dtUntil > parent.Max)
                        dtUntil = parent.Max;

                    if (dtUntil != dtFrom)
                        parent.AddChild("{0:00} ({1:g} - {2:g})".FormatInvariant(GetIso8601WeekOfYear(week), dtFrom, dtUntil.AddDays(-1)),
                            dtFrom, dtUntil);
                }
            }

            return lst.SelectMany(i => i.Children).ToList();
        }

        /// <summary>
        /// Build days from the given time dimensions
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static List<DimensionEntry<DateTime>> BuildDays(this List<DimensionEntry<DateTime>> lst)
        {
            if (lst == null)
                throw new ArgumentNullException(nameof(lst));

            foreach (var parent in lst)
            {
                for (DateTime dtFrom = parent.Min.Date; dtFrom <= parent.Max; dtFrom = dtFrom.AddDays(1))
                {
                    if (dtFrom < parent.Min)
                        dtFrom = parent.Min;

                    var dtUntil = dtFrom.AddDays(1);
                    if (dtUntil > parent.Max)
                        dtUntil = parent.Max;

                    if (dtUntil != dtFrom)
                        parent.AddChild(dtFrom.ToString("d", CultureInfo.InvariantCulture), dtFrom, dtUntil);
                }
            }

            return lst.SelectMany(i => i.Children).ToList();
        }

        // http://blogs.msdn.com/b/shawnste/archive/2006/01/24/iso-8601-week-of-year-format-in-microsoft-net.aspx
        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        private static int GetIso8601WeekOfYear(DateTime time)
        {
            var calendar = CultureInfo.InvariantCulture.Calendar;

            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            var day = calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                time = time.AddDays(3);

            // Return the week of our adjusted day
            return calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}