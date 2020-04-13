using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;

namespace dasz.LinqCube.Tests.DimensionEntryExtensions
{
    [TestClass]
    public class EnumDimensionEntryExtensionsTests
    {

        #region String Dimension
        private static readonly string[] BaseStringGenderArray = { "M", "F" };

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StringDimensionExtensionUsageWhenDimensionIsNull()
        {
            Dimension<string, Person> dimension = null;
            dimension.BuildEnum();
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StringDimensionExtensionUsageWhenParamsIsNull()
        {
            var dimension = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender);
            dimension.BuildEnum(null);
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void StringDimensionIsCorrectParentAssigned()
        {
            var dimension = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender);
            var dimensionEntries = dimension.BuildEnum(BaseStringGenderArray);

            foreach (var dimensionEntry in dimensionEntries)
            {
                if (dimensionEntry.Parent != dimension)
                    Assert.Fail("Parent is not the same as initialized");
            }
        }

        [TestMethod]
        public void StringDimensionAreChildrenOfSameType()
        {
            var dimensionEntries = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender)
                .BuildEnum(BaseStringGenderArray);
            CollectionAssert.AllItemsAreInstancesOfType(dimensionEntries, typeof(DimensionEntry<string>));
        }

        [TestMethod]
        public void StringDimensionAreChildrenUniqueObjects()
        {
            var dimensionEntries = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender)
                .BuildEnum(BaseStringGenderArray);
            CollectionAssert.AllItemsAreUnique(dimensionEntries);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StringDimensionDoNotAllowDuplicateEnumValues()
        {
            var dimensionEntries = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender)
                .BuildEnum("M", "M");
            Assert.Fail("The expected exception was not thrown.");
        }

        [DataTestMethod]
        [DataRow(DisplayName = "No Child")]
        [DataRow("M", DisplayName = "One Child")]
        [DataRow("M", "F", DisplayName = "Two Children")]
        public void StringDimensionAreThereChildrenAsInputProvided(params string[] inputs)
        {
            var dimensionEntries = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender)
                .BuildEnum(inputs);

            Assert.AreEqual(dimensionEntries.Count, inputs.Length);
        }

        [DataTestMethod]
        [DataRow(DisplayName = "No Child")]
        [DataRow("M", DisplayName = "One Child")]
        [DataRow("M", "F", DisplayName = "Two Children")]
        public void StringDimensionAreChildrenWithCorrectValueMinMax(params string[] inputs)
        {
            var dimensionEntries = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender)
                .BuildEnum(inputs);

            int result = 0;
            foreach (var input in inputs)
                result += dimensionEntries.CountChildrenContaining(input);

            Assert.AreEqual(result, inputs.Length);
        }
        #endregion

        #region Enum Dimension
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnumDimensionExtensionUsageWhenDimensionIsNull()
        {
            Dimension<Gender, Person> dimension = null;
            dimension.BuildEnum();
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void EnumDimensionIsCorrectParentAssigned()
        {
            var dimension = new Dimension<Gender, Person>(MethodBase.GetCurrentMethod().Name, k => k.GenderEnum);
            var dimensionEntries = dimension.BuildEnum();

            foreach (var dimensionEntry in dimensionEntries)
            {
                if (dimensionEntry.Parent != dimension)
                    Assert.Fail("Parent is not the same as initialized");
            }
        }

        [TestMethod]
        public void EnumDimensionAreChildrenOfSameType()
        {
            var dimensionEntries = new Dimension<Gender, Person>(MethodBase.GetCurrentMethod().Name, k => k.GenderEnum)
                .BuildEnum();
            CollectionAssert.AllItemsAreInstancesOfType(dimensionEntries, typeof(DimensionEntry<Gender>));
        }

        [TestMethod]
        public void EnumDimensionAreChildrenUnique()
        {
            var dimensionEntries = new Dimension<Gender, Person>(MethodBase.GetCurrentMethod().Name, k => k.GenderEnum)
                .BuildEnum();
            CollectionAssert.AllItemsAreUnique(dimensionEntries);
        }

        [TestMethod]
        public void EnumDimensionAreThereChildrenAsGender0()
        {
            var dimensionEntries = new Dimension<Gender0, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender0Enum)
                .BuildEnum();

            var values = Enum.GetValues(typeof(Gender0)).Cast<Gender0>();
            var result = 0;
            foreach (var value in values)
                result += dimensionEntries.CountChildrenContaining(value);

            Assert.AreEqual(values.Count(), result);
        }


        [TestMethod]
        public void EnumDimensionAreThereChildrenAsGender1()
        {
            var dimensionEntries = new Dimension<Gender1, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender1Enum)
                .BuildEnum();

            var values = Enum.GetValues(typeof(Gender1)).Cast<Gender1>();
            var result = 0;
            foreach (var value in values)
                result += dimensionEntries.CountChildrenContaining(value);

            Assert.AreEqual(values.Count(), result);
        }

        [TestMethod]
        public void EnumDimensionAreThereChildrenAsGender2()
        {
            var dimensionEntries = new Dimension<Gender2, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender2Enum)
                .BuildEnum();

            var values = Enum.GetValues(typeof(Gender2)).Cast<Gender2>();
            var result = 0;
            foreach (var value in values)
                result += dimensionEntries.CountChildrenContaining(value);

            Assert.AreEqual(values.Count(), result);
        }
        #endregion
    }
}