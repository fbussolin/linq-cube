using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Reflection;

namespace dasz.LinqCube.Tests.Dimensions
{
    [TestClass]
    public class DimensionEntryTests
    {
        #region Name Tests
        [TestMethod]
        public void NameIsAssignedCorrectly()
        {
            var name = "CheckIfNameIsAssignedCorrectly";
            var dimensionEntry = new DimensionEntry<DateTime>(name);
            StringAssert.Contains(dimensionEntry.Name, name);
        }

        [DataTestMethod]
        [DataRow(null, DisplayName = "null")]
        [DataRow("", DisplayName = "Empty")]
        [DataRow("         ", DisplayName = "Whitespaces")]
        [ExpectedException(typeof(ArgumentException))]
        public void NameValidation(string name)
        {
            var dimensionEntry = new DimensionEntry<DateTime>(name);
            Assert.Fail("The expected exception was not thrown.");
        }
        #endregion

        #region Parent Tests
        [TestMethod]
        public void ParentIsTheDimensionSet()
        {
            var dimension = new Dimension<DateTime, Person>(MethodBase.GetCurrentMethod().Name, s => s.Birthday);
            var dimensionEntry = new DimensionEntry<DateTime>(MethodBase.GetCurrentMethod().Name, dimension);
            Assert.AreEqual(dimensionEntry.Parent, dimension);
        }
        #endregion

        #region Root Tests
        [TestMethod]
        public void IsNotDimensionRootAndParentIsNullImplicit()
        {
            var dimensionEntry = new DimensionEntry<DateTime>(MethodBase.GetCurrentMethod().Name);
            Assert.AreNotEqual(dimensionEntry.Root, dimensionEntry);
        }

        [TestMethod]
        public void IsNotDimensionRootAndParentIsNullExplicit()
        {
            var dimensionEntry = new DimensionEntry<DateTime>(MethodBase.GetCurrentMethod().Name, null);
            Assert.AreNotEqual(dimensionEntry.Root, dimensionEntry);
        }

        [TestMethod]
        public void IsNotDimensionRootAndParentIsDimension()
        {
            var dimension = new Dimension<DateTime, Person>(MethodBase.GetCurrentMethod().Name, s => s.Birthday);
            var dimensionEntry = new DimensionEntry<DateTime>(MethodBase.GetCurrentMethod().Name, dimension);
            Assert.AreNotEqual(dimensionEntry.Root, dimensionEntry);
        }

        [TestMethod]
        public void RootIsTheDimensionRootSet()
        {
            var dimension = new Dimension<DateTime, Person>(MethodBase.GetCurrentMethod().Name, s => s.Birthday);
            var dimensionEntry1 = new DimensionEntry<DateTime>($"{MethodBase.GetCurrentMethod().Name}1", dimension);
            var dimensionEntry2 = new DimensionEntry<DateTime>($"{MethodBase.GetCurrentMethod().Name}2", dimensionEntry1);
            var dimensionEntry3 = new DimensionEntry<DateTime>($"{MethodBase.GetCurrentMethod().Name}3", dimensionEntry2);
            var dimensionEntry4 = new DimensionEntry<DateTime>($"{MethodBase.GetCurrentMethod().Name}4", dimensionEntry3);
            var dimensionEntry5 = new DimensionEntry<DateTime>($"{MethodBase.GetCurrentMethod().Name}5", dimensionEntry4);
            Assert.AreEqual(dimensionEntry5.Root, dimension);
        }
        #endregion

        #region InRange Single Value Tests
        [DataTestMethod]
        [DataRow(null, DisplayName = "InRange Argument null")]
        [DataRow("InRangeArgument", DisplayName = "InRange Argument Set")]
        public void InRangeValueNotSetMinMaxNotSetArgumentData(string inRange)
        {
            var dimensionEntry = new DimensionEntry<string>(MethodBase.GetCurrentMethod().Name);
            Assert.IsFalse(dimensionEntry.InRange(inRange));
        }

        [DataTestMethod]
        [DataRow(null, null, false, DisplayName = "Both null")]
        [DataRow(null, "InRangeArgument", false, DisplayName = "Value null")]
        [DataRow("InRangeValue", null, false, DisplayName = "Argument null")]
        [DataRow("InRangeTest", "InRangeTest", true, DisplayName = "Equals")]
        public void InRangeValueDataMinMaxNotSetArgumentData(string value, string inRange, bool result)
        {
            var dimensionEntry = new DimensionEntry<string>(MethodBase.GetCurrentMethod().Name);
            dimensionEntry.Value = value;
            Assert.AreEqual(dimensionEntry.InRange(inRange), result);
        }

        [DataTestMethod]
        [DataRow(null, false, DisplayName = "null")]
        [DataRow(default(string), false, DisplayName = "default")]
        [DataRow("AA", false, DisplayName = "Before")]
        [DataRow("ZZ", false, DisplayName = "After")]
        [DataRow("CC", true, DisplayName = "Start Touching")]
        [DataRow("XX", true, DisplayName = "End Touching")]
        [DataRow("MM", true, DisplayName = "Inside")]
        public void InRangeValueNotSetMinMaxAreSetArgumentData(string inRange, bool result)
        {
            var dimensionEntry = new DimensionEntry<string>(MethodBase.GetCurrentMethod().Name);
            dimensionEntry.Min = "CC";
            dimensionEntry.Max = "XX";
            Assert.AreEqual(dimensionEntry.InRange(inRange), result);
        }

        [DataTestMethod]
        [DataRow(null, null, false, DisplayName = "Both null")]
        [DataRow(null, "AA", false, DisplayName = "Value null and Argument Before")]
        [DataRow(null, "ZZ", false, DisplayName = "Value null and Argument After")]
        [DataRow(null, "CC", true, DisplayName = "Value null and Argument Start Touching")]
        [DataRow(null, "XX", true, DisplayName = "Value null and Argument End Touching")]
        [DataRow(null, "MM", true, DisplayName = "Value null and Argument Inside")]
        [DataRow("MM", null, false, DisplayName = "Value Set and Argument null")]
        [DataRow("MM", "MM", true, DisplayName = "Value Set and Argument Set")]
        public void InRangeValueDataMinMaxAreSetArgumentData(string value, string inRange, bool result)
        {
            var dimensionEntry = new DimensionEntry<string>(MethodBase.GetCurrentMethod().Name);
            dimensionEntry.Value = value;
            dimensionEntry.Min = "CC";
            dimensionEntry.Max = "XX";
            Assert.AreEqual(dimensionEntry.InRange(inRange), result);
        }
        #endregion

        #region InRange Lower Upper Tests

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InRangeLowerUpperValueIsSetMinMaxNotSet()
        {
            var dimensionEntry = new DimensionEntry<string>(MethodBase.GetCurrentMethod().Name);
            dimensionEntry.Value = "M";
            dimensionEntry.InRange("A", "Z");
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void InRangeLowerUpperValueNotSetMinMaxNotSet()
        {
            var dimensionEntry = new DimensionEntry<string>(MethodBase.GetCurrentMethod().Name);
            Assert.IsFalse(dimensionEntry.InRange("A", "Z"));
        }

        [TestMethod]
        public void InRangeLowerUpperValueIsNullMinMaxNotSet()
        {
            var dimensionEntry = new DimensionEntry<string>(MethodBase.GetCurrentMethod().Name);
            dimensionEntry.Value = null;
            Assert.IsFalse(dimensionEntry.InRange("A", "Z"));
        }

        [DataTestMethod]
        [DataRow(null, null, DisplayName = "Both null")]
        [DataRow(null, "ZZ", DisplayName = "Lower null")]
        [DataRow("AA", null, DisplayName = "Upper null")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InRangeLowerUpperArgumentsNullsMinMaxNotSet(string lower, string upper)
        {
            var dimensionEntry = new DimensionEntry<string>(MethodBase.GetCurrentMethod().Name);
            dimensionEntry.InRange(lower, upper);
            Assert.Fail("The expected exception was not thrown.");
        }

        #region Period Relations
        /// <image url="$(SolutionDir)Images\Period Relations.png" scale="1.0" />

        private bool RegularPeriodRelationsHelper(DateTime startB, DateTime endB)
        {
            var dimensionEntry = new DimensionEntry<DateTime>(MethodBase.GetCurrentMethod().Name);
            dimensionEntry.Min = new DateTime(2020, 01, 01);
            dimensionEntry.Max = new DateTime(2020, 12, 31);
            return dimensionEntry.InRange(startB, endB);
        }

        private bool InversePeriodRelationsHelper(DateTime startB, DateTime endB)
        {
            var dimensionEntry = new DimensionEntry<DateTime>(MethodBase.GetCurrentMethod().Name);
            dimensionEntry.Min = new DateTime(2020, 12, 31);
            dimensionEntry.Max = new DateTime(2020, 01, 01);
            return dimensionEntry.InRange(startB, endB);
        }

        [DataTestMethod]
        [DataRow("2019-09-09", "2019-12-31", false, DisplayName = "After")]
        [DataRow("2019-09-09", "2020-01-01", true, DisplayName = "Start Touching")]
        [DataRow("2019-09-09", "2020-01-02", true, DisplayName = "Start Inside")]
        [DataRow("2020-01-01", "2021-04-03", true, DisplayName = "Inside Start Touching")]
        [DataRow("2020-01-01", "2020-09-09", true, DisplayName = "Enclosing Start Touching")]
        [DataRow("2020-04-03", "2020-09-09", true, DisplayName = "Enclosing")]
        [DataRow("2020-04-03", "2020-12-31", true, DisplayName = "Enclosing End Touching")]
        [DataRow("2020-01-01", "2020-12-31", true, DisplayName = "Exact Match")]
        [DataRow("2019-09-09", "2021-04-03", true, DisplayName = "Inside")]
        [DataRow("2019-09-09", "2020-12-31", true, DisplayName = "Inside End Touching")]
        [DataRow("2020-12-30", "2021-04-03", true, DisplayName = "End Inside")]
        [DataRow("2020-12-31", "2021-04-03", true, DisplayName = "End Touching")]
        [DataRow("2021-01-01", "2021-04-03", false, DisplayName = "Before")]
        public void InRangeLowerUpperRegularPeriodRelations(string start, string end, bool result)
        {
            var startB = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endB = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Assert.AreEqual(RegularPeriodRelationsHelper(startB, endB), result);
        }

        [DataTestMethod]
        [DataRow("2019-09-09", "2019-12-31", false, DisplayName = "After")]
        [DataRow("2019-09-09", "2020-01-01", false, DisplayName = "Start Touching")]
        [DataRow("2019-09-09", "2020-01-02", false, DisplayName = "Start Inside")]
        [DataRow("2020-01-01", "2021-04-03", false, DisplayName = "Inside Start Touching")]
        [DataRow("2020-01-01", "2020-09-09", false, DisplayName = "Enclosing Start Touching")]
        [DataRow("2020-04-03", "2020-09-09", false, DisplayName = "Enclosing")]
        [DataRow("2020-04-03", "2020-12-31", false, DisplayName = "Enclosing End Touching")]
        [DataRow("2020-01-01", "2020-12-31", false, DisplayName = "Exact Match")]
        [DataRow("2019-09-09", "2021-04-03", false, DisplayName = "Inside")]
        [DataRow("2019-09-09", "2020-12-31", false, DisplayName = "Inside End Touching")]
        [DataRow("2020-12-30", "2021-04-03", false, DisplayName = "End Inside")]
        [DataRow("2020-12-31", "2021-04-03", false, DisplayName = "End Touching")]
        [DataRow("2021-01-01", "2021-04-03", false, DisplayName = "Before")]
        public void InRangeLowerUpperInverseAPeriodRelations(string start, string end, bool result)
        {
            var startB = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endB = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Assert.AreEqual(InversePeriodRelationsHelper(startB, endB), result);
        }

        [DataTestMethod]
        [DataRow("2019-12-31", "2019-09-09", false, DisplayName = "After")]
        [DataRow("2020-01-01", "2019-09-09", false, DisplayName = "Start Touching")]
        [DataRow("2020-01-02", "2019-09-09", false, DisplayName = "Start Inside")]
        [DataRow("2021-04-03", "2020-01-01", false, DisplayName = "Inside Start Touching")]
        [DataRow("2020-09-09", "2020-01-01", false, DisplayName = "Enclosing Start Touching")]
        [DataRow("2020-09-09", "2020-04-03", false, DisplayName = "Enclosing")]
        [DataRow("2020-12-31", "2020-04-03", false, DisplayName = "Enclosing End Touching")]
        [DataRow("2020-12-31", "2020-01-01", false, DisplayName = "Exact Match")]
        [DataRow("2021-04-03", "2019-09-09", false, DisplayName = "Inside")]
        [DataRow("2020-12-31", "2019-09-09", false, DisplayName = "Inside End Touching")]
        [DataRow("2021-04-03", "2020-12-30", false, DisplayName = "End Inside")]
        [DataRow("2021-04-03", "2020-12-31", false, DisplayName = "End Touching")]
        [DataRow("2021-04-03", "2021-01-01", false, DisplayName = "Before")]
        public void InRangeLowerUpperInverseBPeriodRelations(string start, string end, bool result)
        {
            var startB = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endB = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Assert.AreEqual(RegularPeriodRelationsHelper(startB, endB), result);
        }

        [DataTestMethod]
        [DataRow("2019-12-31", "2019-09-09", false, DisplayName = "After")]
        [DataRow("2020-01-01", "2019-09-09", false, DisplayName = "Start Touching")]
        [DataRow("2020-01-02", "2019-09-09", false, DisplayName = "Start Inside")]
        [DataRow("2021-04-03", "2020-01-01", false, DisplayName = "Inside Start Touching")]
        [DataRow("2020-09-09", "2020-01-01", false, DisplayName = "Enclosing Start Touching")]
        [DataRow("2020-09-09", "2020-04-03", false, DisplayName = "Enclosing")]
        [DataRow("2020-12-31", "2020-04-03", false, DisplayName = "Enclosing End Touching")]
        [DataRow("2020-12-31", "2020-01-01", false, DisplayName = "Exact Match")]
        [DataRow("2021-04-03", "2019-09-09", false, DisplayName = "Inside")]
        [DataRow("2020-12-31", "2019-09-09", false, DisplayName = "Inside End Touching")]
        [DataRow("2021-04-03", "2020-12-30", false, DisplayName = "End Inside")]
        [DataRow("2021-04-03", "2020-12-31", false, DisplayName = "End Touching")]
        [DataRow("2021-04-03", "2021-01-01", false, DisplayName = "Before")]
        public void InRangeLowerUpperInverseABPeriodRelations(string start, string end, bool result)
        {
            var startB = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endB = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Assert.AreEqual(InversePeriodRelationsHelper(startB, endB), result);
        }
        #endregion

        #endregion

        [TestMethod]
        public void TestToString()
        {
            var dimensionEntry = new DimensionEntry<DateTime>(MethodBase.GetCurrentMethod().Name);
            var dimensionEntryToString = $"DimensionEntry: {MethodBase.GetCurrentMethod().Name} {{Value:{dimensionEntry.Value},Min:{dimensionEntry.Min},Max:{dimensionEntry.Max}}}";
            StringAssert.Contains(dimensionEntry.ToString(), dimensionEntryToString);
        }
    }
}