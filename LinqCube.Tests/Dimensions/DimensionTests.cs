using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace dasz.LinqCube.Tests.Dimensions
{
    [TestClass]
    public class DimensionTests
    {
        [TestMethod]
        public void IsDimensionRoot()
        {
            var dimension = new Dimension<DateTime, Person>(MethodBase.GetCurrentMethod().Name, s => s.Birthday);
            Assert.AreEqual(dimension.Root, dimension);
        }

        [TestMethod]
        public void NameIsAssignedCorrectly()
        {
            var name = MethodBase.GetCurrentMethod().Name;
            var dimension = new Dimension<DateTime, Person>(name, s => s.Birthday);
            StringAssert.Contains(dimension.Name, name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameCannotBeNull()
        {
            var dimension = new Dimension<DateTime, Person>(null, s => s.Birthday);
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameCannotBeEmtpy()
        {
            var dimension = new Dimension<DateTime, Person>(string.Empty, s => s.Birthday);
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameCannotBeWhitespaces()
        {
            var dimension = new Dimension<DateTime, Person>("         ", s => s.Birthday);
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void GetMinWhenThereIsNoChildren()
        {
            var dimension = new Dimension<DateTime, Person>(MethodBase.GetCurrentMethod().Name, s => s.Birthday);
            Equals(dimension.Min, default(DateTime));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetMinInDimension()
        {
            var dimension = new Dimension<DateTime, Person>(MethodBase.GetCurrentMethod().Name, s => s.Birthday);
            dimension.Min = new DateTime(2299, 1, 1);
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void GetMaxWhenThereIsNoChildren()
        {
            var dimension = new Dimension<DateTime, Person>(MethodBase.GetCurrentMethod().Name, s => s.Birthday);
            Equals(dimension.Max, default(DateTime));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetMaxInDimension()
        {
            var dimension = new Dimension<DateTime, Person>(MethodBase.GetCurrentMethod().Name, s => s.Birthday);
            dimension.Max = new DateTime(2299, 1, 1);
            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        public void InRangeWhenValueIsAssigned()
        {
            var dimension = new Dimension<DateTime, Person>(MethodBase.GetCurrentMethod().Name, s => s.Birthday);
            dimension.Value = new DateTime(2299, 1, 1);
            Assert.IsTrue(dimension.InRange(new DateTime(2299, 1, 1)));
        }

        [TestMethod]
        public void InRangeWhenValueIsNotAssigned()
        {
            var dimension = new Dimension<DateTime, Person>(MethodBase.GetCurrentMethod().Name, s => s.Birthday);
            Assert.IsFalse(dimension.InRange(new DateTime(2299, 1, 1)));
        }

        [TestMethod]
        public void InRangeDefaultDateTimeWhenValueIsNotAssigned()
        {
            var dimension = new Dimension<DateTime, Person>(MethodBase.GetCurrentMethod().Name, s => s.Birthday);
            Assert.IsFalse(dimension.InRange(default));
        }

        [TestMethod]
        public void TestToString()
        {
            var dimension = new Dimension<DateTime, Person>(MethodBase.GetCurrentMethod().Name, s => s.Birthday);
            var dimensionToString = $"Dimension: {MethodBase.GetCurrentMethod().Name} {{Value:{dimension.Value},Min:{dimension.Min},Max:{dimension.Max}}}";
            StringAssert.Contains(dimension.ToString(), dimensionToString);
        }
    }
}