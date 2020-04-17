using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dasz.LinqCube.Tests.DimensionEntryExtensions
{
    [TestClass]
    public class IntDimensionEntryExtensionsTests
    {
        //[TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void ExtensionUsageWhenDimensionIsNull()
        //{
        //    Dimension<int, Person> dimension = null;
        //    dimension.BuildEnum();
        //    Assert.Fail("The expected exception was not thrown.");
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void StringDimensionExtensionUsageWhenParamsIsNull()
        //{
        //    var dimension = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender);
        //    dimension.BuildEnum(null);
        //    Assert.Fail("The expected exception was not thrown.");
        //}

        //[TestMethod]
        //public void StringDimensionIsCorrectParentAssigned()
        //{
        //    var dimension = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender);
        //    var dimensionEntries = dimension.BuildEnum(BaseStringGenderArray);

        //    foreach (var dimensionEntry in dimensionEntries)
        //    {
        //        if (dimensionEntry.Parent != dimension)
        //            Assert.Fail("Parent is not the same as initialized");
        //    }
        //}

        //[TestMethod]
        //public void StringDimensionAreChildrenOfSameType()
        //{
        //    var dimensionEntries = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender)
        //        .BuildEnum(BaseStringGenderArray);
        //    CollectionAssert.AllItemsAreInstancesOfType(dimensionEntries, typeof(DimensionEntry<string>));
        //}

        //[TestMethod]
        //public void StringDimensionAreChildrenUniqueObjects()
        //{
        //    var dimensionEntries = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender)
        //        .BuildEnum(BaseStringGenderArray);
        //    CollectionAssert.AllItemsAreUnique(dimensionEntries);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void StringDimensionDoNotAllowDuplicateEnumValues()
        //{
        //    var dimensionEntries = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender)
        //        .BuildEnum("M", "M");
        //    Assert.Fail("The expected exception was not thrown.");
        //}

        //[DataTestMethod]
        //[DataRow(DisplayName = "No Child")]
        //[DataRow("M", DisplayName = "One Child")]
        //[DataRow("M", "F", DisplayName = "Two Children")]
        //public void StringDimensionAreThereChildrenAsInputProvided(params string[] inputs)
        //{
        //    var dimensionEntries = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender)
        //        .BuildEnum(inputs);

        //    Assert.AreEqual(dimensionEntries.Count, inputs.Length);
        //}

        //[DataTestMethod]
        //[DataRow(DisplayName = "No Child")]
        //[DataRow("M", DisplayName = "One Child")]
        //[DataRow("M", "F", DisplayName = "Two Children")]
        //public void StringDimensionAreChildrenWithCorrectValueMinMax(params string[] inputs)
        //{
        //    var dimensionEntries = new Dimension<string, Person>(MethodBase.GetCurrentMethod().Name, k => k.Gender)
        //        .BuildEnum(inputs);

        //    int result = 0;
        //    foreach (var input in inputs)
        //        result += dimensionEntries.CountChildrenContaining(input, null, null);

        //    Assert.AreEqual(result, inputs.Length);
        //}
    }
}