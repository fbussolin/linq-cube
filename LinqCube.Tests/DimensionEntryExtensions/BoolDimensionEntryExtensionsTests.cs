using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace dasz.LinqCube.Tests.DimensionEntryExtensions
{
    [TestClass]
    public class BoolDimensionEntryExtensionsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtensionUsageForNullDimension()
        {
            Dimension<bool, Person> dimension = null;
            dimension.BuildBool();
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void IsCorrectParentAssigned()
        {
            var dimension = new Dimension<bool, Person>(MethodBase.GetCurrentMethod().Name, k => k.Active);
            var dimensionEntries = dimension.BuildBool();

            foreach (var dimensionEntry in dimensionEntries)
            {
                if (dimensionEntry.Parent != dimension)
                    Assert.Fail("Parent is not the same as initialized");
            }
        }

        [TestMethod]
        public void AreChildrenOfSameType()
        {
            var dimensionEntries = new Dimension<bool, Person>(MethodBase.GetCurrentMethod().Name, k => k.Active)
                .BuildBool();
            CollectionAssert.AllItemsAreInstancesOfType(dimensionEntries, typeof(DimensionEntry<Boolean>));
        }

        [TestMethod]
        public void AreChildrenUniqueObjects()
        {
            var dimensionEntries = new Dimension<bool, Person>(MethodBase.GetCurrentMethod().Name, k => k.Active)
                .BuildBool();
            CollectionAssert.AllItemsAreUnique(dimensionEntries);
        }

        [TestMethod]
        public void AreThereExactlyTwoChildren()
        {
            var dimensionEntries = new Dimension<bool, Person>(MethodBase.GetCurrentMethod().Name, k => k.Active)
                .BuildBool();

            Assert.AreEqual(dimensionEntries.Count, 2);
        }

        [DataTestMethod]
        [DataRow(true, DisplayName = "True")]
        [DataRow(false, DisplayName = "False")]
        public void AreChildrenWithCorrectValueMinMax(bool input)
        {
            var dimensionEntries = new Dimension<bool, Person>(MethodBase.GetCurrentMethod().Name, k => k.Active)
                .BuildBool();

            Assert.AreEqual(dimensionEntries.CountChildrenContaining(input), 1);
        }
    }
}