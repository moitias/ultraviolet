﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class CircleDTests : UltravioletTestFramework
    {
        [TestMethod]
        public void CircleD_IsConstructedProperly()
        {
            var result = new CircleD(123.45, 456.78, 100.10);

            TheResultingValue(result)
                .ShouldHavePosition(123.45, 456.78)
                .ShouldHaveRadius(100.10);
        }

        [TestMethod]
        public void CircleD_OpEquality()
        {
            var circle1 = new CircleD(123.45, 456.78, 100.10);
            var circle2 = new CircleD(123.45, 456.78, 100.10);
            var circle3 = new CircleD(123.45, 555, 100.10);
            var circle4 = new CircleD(222, 456.78, 100.10);
            var circle5 = new CircleD(123.45, 456.78, 200);

            TheResultingValue(circle1 == circle2).ShouldBe(true);
            TheResultingValue(circle1 == circle3).ShouldBe(false);
            TheResultingValue(circle1 == circle4).ShouldBe(false);
            TheResultingValue(circle1 == circle5).ShouldBe(false);
        }

        [TestMethod]
        public void CircleD_OpInequality()
        {
            var circle1 = new CircleD(123.45, 456.78, 100.10);
            var circle2 = new CircleD(123.45, 456.78, 100.10);
            var circle3 = new CircleD(123.45, 555, 100.10);
            var circle4 = new CircleD(222, 456.78, 100.10);
            var circle5 = new CircleD(123.45, 456.78, 200);

            TheResultingValue(circle1 != circle2).ShouldBe(false);
            TheResultingValue(circle1 != circle3).ShouldBe(true);
            TheResultingValue(circle1 != circle4).ShouldBe(true);
            TheResultingValue(circle1 != circle5).ShouldBe(true);
        }

        [TestMethod]
        public void CircleD_EqualsObject()
        {
            var circle1 = new CircleD(123.45, 456.78, 100.10);
            var circle2 = new CircleD(123.45, 456.78, 100.10);

            TheResultingValue(circle1.Equals((Object)circle2)).ShouldBe(true);
            TheResultingValue(circle1.Equals("This is a test")).ShouldBe(false);
        }

        [TestMethod]
        public void CircleD_EqualsCircleD()
        {
            var circle1 = new CircleD(123.45, 456.78, 100.10);
            var circle2 = new CircleD(123.45, 456.78, 100.10);
            var circle3 = new CircleD(123.45, 555.55, 100.10);
            var circle4 = new CircleD(222.22, 456.78, 100.10);

            TheResultingValue(circle1.Equals(circle2)).ShouldBe(true);
            TheResultingValue(circle1.Equals(circle3)).ShouldBe(false);
            TheResultingValue(circle1.Equals(circle4)).ShouldBe(false);
        }

        [TestMethod]
        public void CircleD_TryParse_SucceedsForValidStrings()
        {
            var str = "123.45 456.78 100.10";
            var result = default(CircleD);
            if (!CircleD.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to CircleD.");

            TheResultingValue(result)
                .ShouldHavePosition(123.45, 456.78)
                .ShouldHaveRadius(100.10);
        }

        [TestMethod]
        public void CircleD_TryParse_FailsForInvalidStrings()
        {
            var result    = default(CircleD);
            var succeeded = CircleD.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [TestMethod]
        public void CircleD_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 100.10";
            var result = CircleD.Parse(str);

            TheResultingValue(result)
                .ShouldHavePosition(123.45, 456.78)
                .ShouldHaveRadius(100.10);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void CircleD_Parse_FailsForInvalidStrings()
        {
            CircleD.Parse("foo");
        }

        [TestMethod]
        public void CircleD_Parse_CanRoundTrip()
        {
            var circle1 = CircleD.Parse("123.4 456.7 100");
            var circle2 = CircleD.Parse(circle1.ToString());

            TheResultingValue(circle1 == circle2).ShouldBe(true);
        }
    }
}
