﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class RectangleFTests : UltravioletTestFramework
    {
        [TestMethod]
        public void RectangleF_IsConstructedProperly()
        {
            var result = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);

            TheResultingValue(result)
                .ShouldHavePosition(123.45f, 456.78f)
                .ShouldHaveDimensions(789.99f, 999.99f);
        }

        [TestMethod]
        public void RectangleF_OpEquality()
        {
            var rectangle1 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle2 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle3 = new RectangleF(222f, 456.78f, 789.99f, 999.99f);
            var rectangle4 = new RectangleF(123.45f, 333f, 789.99f, 999.99f);
            var rectangle5 = new RectangleF(123.45f, 456.78f, 444f, 999.99f);
            var rectangle6 = new RectangleF(123.45f, 456.78f, 789.99f, 555f);

            Assert.AreEqual(true, rectangle1 == rectangle2);
            Assert.AreEqual(false, rectangle1 == rectangle3);
            Assert.AreEqual(false, rectangle1 == rectangle4);
            Assert.AreEqual(false, rectangle1 == rectangle5);
            Assert.AreEqual(false, rectangle1 == rectangle6);
        }

        [TestMethod]
        public void RectangleF_OpInequality()
        {
            var rectangle1 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle2 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle3 = new RectangleF(222f, 456.78f, 789.99f, 999.99f);
            var rectangle4 = new RectangleF(123.45f, 333f, 789.99f, 999.99f);
            var rectangle5 = new RectangleF(123.45f, 456.78f, 444f, 999.99f);
            var rectangle6 = new RectangleF(123.45f, 456.78f, 789.99f, 555f);

            Assert.AreEqual(false, rectangle1 != rectangle2);
            Assert.AreEqual(true, rectangle1 != rectangle3);
            Assert.AreEqual(true, rectangle1 != rectangle4);
            Assert.AreEqual(true, rectangle1 != rectangle5);
            Assert.AreEqual(true, rectangle1 != rectangle6);
        }

        [TestMethod]
        public void RectangleF_EqualsObject()
        {
            var rectangle1 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle2 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);

            TheResultingValue(rectangle1.Equals((Object)rectangle2)).ShouldBe(true);
            TheResultingValue(rectangle1.Equals("This is a test")).ShouldBe(false);
        }

        [TestMethod]
        public void RectangleF_EqualsRectangleF()
        {
            var rectangle1 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle2 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle3 = new RectangleF(222f, 456.78f, 789.99f, 999.99f);
            var rectangle4 = new RectangleF(123.45f, 333f, 789.99f, 999.99f);
            var rectangle5 = new RectangleF(123.45f, 456.78f, 444f, 999.99f);
            var rectangle6 = new RectangleF(123.45f, 456.78f, 789.99f, 555f);

            TheResultingValue(rectangle1.Equals(rectangle2)).ShouldBe(true);
            TheResultingValue(rectangle1.Equals(rectangle3)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle4)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle5)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle6)).ShouldBe(false);
        }

        [TestMethod]
        public void RectangleF_TryParse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 789.99 999.99";
            var result = default(RectangleF);
            if (!RectangleF.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to RectangleF.");

            TheResultingValue(result)
                .ShouldHavePosition(123.45f, 456.78f)
                .ShouldHaveDimensions(789.99f, 999.99f);
        }

        [TestMethod]
        public void RectangleF_TryParse_FailsForInvalidStrings()
        {
            var result    = default(RectangleF);
            var succeeded = RectangleF.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [TestMethod]
        public void RectangleF_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 789.99 999.99";
            var result = RectangleF.Parse(str);

            TheResultingValue(result)
                .ShouldHavePosition(123.45f, 456.78f)
                .ShouldHaveDimensions(789.99f, 999.99f);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void RectangleF_Parse_FailsForInvalidStrings()
        {
            RectangleF.Parse("foo");
        }

        [TestMethod]
        public void RectangleF_Parse_CanRoundTrip()
        {
            var rect1 = RectangleF.Parse("123.4 456.7 789.1 234.5");
            var rect2 = RectangleF.Parse(rect1.ToString());

            TheResultingValue(rect1 == rect2).ShouldBe(true);
        }
    }
}
