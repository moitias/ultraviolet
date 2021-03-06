﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class RectangleDTests : UltravioletTestFramework
    {
        [TestMethod]
        public void RectangleD_IsConstructedProperly()
        {
            var result = new RectangleD(123.45, 456.78, 789.99, 999.99);

            TheResultingValue(result)
                .ShouldHavePosition(123.45, 456.78)
                .ShouldHaveDimensions(789.99, 999.99);
        }

        [TestMethod]
        public void RectangleD_OpEquality()
        {
            var rectangle1 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle2 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle3 = new RectangleD(222, 456.78, 789.99, 999.99);
            var rectangle4 = new RectangleD(123.45, 333, 789.99, 999.99);
            var rectangle5 = new RectangleD(123.45, 456.78, 444, 999.99);
            var rectangle6 = new RectangleD(123.45, 456.78, 789.99, 555);

            Assert.AreEqual(true, rectangle1 == rectangle2);
            Assert.AreEqual(false, rectangle1 == rectangle3);
            Assert.AreEqual(false, rectangle1 == rectangle4);
            Assert.AreEqual(false, rectangle1 == rectangle5);
            Assert.AreEqual(false, rectangle1 == rectangle6);
        }

        [TestMethod]
        public void RectangleD_OpInequality()
        {
            var rectangle1 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle2 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle3 = new RectangleD(222, 456.78, 789.99, 999.99);
            var rectangle4 = new RectangleD(123.45, 333, 789.99, 999.99);
            var rectangle5 = new RectangleD(123.45, 456.78, 444, 999.99);
            var rectangle6 = new RectangleD(123.45, 456.78, 789.99, 555);

            Assert.AreEqual(false, rectangle1 != rectangle2);
            Assert.AreEqual(true, rectangle1 != rectangle3);
            Assert.AreEqual(true, rectangle1 != rectangle4);
            Assert.AreEqual(true, rectangle1 != rectangle5);
            Assert.AreEqual(true, rectangle1 != rectangle6);
        }

        [TestMethod]
        public void RectangleD_EqualsObject()
        {
            var rectangle1 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle2 = new RectangleD(123.45, 456.78, 789.99, 999.99);

            TheResultingValue(rectangle1.Equals((Object)rectangle2)).ShouldBe(true);
            TheResultingValue(rectangle1.Equals("This is a test")).ShouldBe(false);
        }

        [TestMethod]
        public void RectangleD_EqualsRectangleD()
        {
            var rectangle1 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle2 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle3 = new RectangleD(222, 456.78, 789.99, 999.99);
            var rectangle4 = new RectangleD(123.45, 333, 789.99, 999.99);
            var rectangle5 = new RectangleD(123.45, 456.78, 444, 999.99);
            var rectangle6 = new RectangleD(123.45, 456.78, 789.99, 555);

            TheResultingValue(rectangle1.Equals(rectangle2)).ShouldBe(true);
            TheResultingValue(rectangle1.Equals(rectangle3)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle4)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle5)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle6)).ShouldBe(false);
        }

        [TestMethod]
        public void RectangleD_TryParse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 789.99 999.99";
            var result = default(RectangleD);
            if (!RectangleD.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to RectangleD.");

            TheResultingValue(result)
                .ShouldHavePosition(123.45, 456.78)
                .ShouldHaveDimensions(789.99, 999.99);
        }

        [TestMethod]
        public void RectangleD_TryParse_FailsForInvalidStrings()
        {
            var result    = default(RectangleD);
            var succeeded = RectangleD.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [TestMethod]
        public void RectangleD_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 789.99 999.99";
            var result = RectangleD.Parse(str);

            TheResultingValue(result)
                .ShouldHavePosition(123.45, 456.78)
                .ShouldHaveDimensions(789.99, 999.99);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void RectangleD_Parse_FailsForInvalidStrings()
        {
            RectangleD.Parse("foo");
        }

        [TestMethod]
        public void RectangleD_Parse_CanRoundTrip()
        {
            var rect1 = RectangleD.Parse("123.4 456.7 789.1 234.5");
            var rect2 = RectangleD.Parse(rect1.ToString());

            TheResultingValue(rect1 == rect2).ShouldBe(true);
        }
    }
}
