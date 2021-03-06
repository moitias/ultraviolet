﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class Size3Tests : UltravioletTestFramework
    {
        [TestMethod]
        public void Size3_IsConstructedProperly()
        {
            var result = new Size3(123, 456, 789);

            TheResultingValue(result)
                .ShouldBe(123, 456, 789);
        }

        [TestMethod]
        public void Size3_OpEquality()
        {
            var volume1 = new Size3(123, 456, 789);
            var volume2 = new Size3(123, 456, 789);
            var volume3 = new Size3(123, 555, 789);
            var volume4 = new Size3(222, 456, 789);
            var volume5 = new Size3(123, 456, 999);

            TheResultingValue(volume1 == volume2).ShouldBe(true);
            TheResultingValue(volume1 == volume3).ShouldBe(false);
            TheResultingValue(volume1 == volume4).ShouldBe(false);
            TheResultingValue(volume1 == volume5).ShouldBe(false);
        }

        [TestMethod]
        public void Size3_OpInequality()
        {
            var volume1 = new Size3(123, 456, 789);
            var volume2 = new Size3(123, 456, 789);
            var volume3 = new Size3(123, 555, 789);
            var volume4 = new Size3(222, 456, 789);
            var volume5 = new Size3(123, 456, 999);

            TheResultingValue(volume1 != volume2).ShouldBe(false);
            TheResultingValue(volume1 != volume3).ShouldBe(true);
            TheResultingValue(volume1 != volume4).ShouldBe(true);
            TheResultingValue(volume1 != volume5).ShouldBe(true);
        }

        [TestMethod]
        public void Size3_EqualsObject()
        {
            var volume1 = new Size3(123, 456, 789);
            var volume2 = new Size3(123, 456, 789);

            TheResultingValue(volume1.Equals((Object)volume2)).ShouldBe(true);
            TheResultingValue(volume1.Equals("This is a test")).ShouldBe(false);
        }

        [TestMethod]
        public void Size3_EqualsSize3()
        {
            var volume1 = new Size3(123, 456, 789);
            var volume2 = new Size3(123, 456, 789);
            var volume3 = new Size3(123, 555, 789);
            var volume4 = new Size3(222, 456, 789);
            var volume5 = new Size3(123, 456, 999);

            TheResultingValue(volume1.Equals(volume2)).ShouldBe(true);
            TheResultingValue(volume1.Equals(volume3)).ShouldBe(false);
            TheResultingValue(volume1.Equals(volume4)).ShouldBe(false);
            TheResultingValue(volume1.Equals(volume5)).ShouldBe(false);
        }

        [TestMethod]
        public void Size3_TryParse_SucceedsForValidStrings()
        {
            var str    = "123 456 789";
            var result = default(Size3);
            if (!Size3.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Size3.");

            TheResultingValue(result)
                .ShouldBe(123, 456, 789);
        }

        [TestMethod]
        public void Size3_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Size3);
            var succeeded = Size3.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [TestMethod]
        public void Size3_Parse_SucceedsForValidStrings()
        {
            var str    = "123 456 789";
            var result = Size3.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123, 456, 789);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Size3_Parse_FailsForInvalidStrings()
        {
            Size3.Parse("foo");
        }

        [TestMethod]
        public void Size3_Parse_CanRoundTrip()
        {
            var size1 = Size3.Parse("123 456 789");
            var size2 = Size3.Parse(size1.ToString());

            TheResultingValue(size1 == size2).ShouldBe(true);
        }

        [TestMethod]
        public void Size3_TotalSize3_IsCalculatedCorrectly()
        {
            var volume1 = new Size3(123, 456, 789);
            TheResultingValue(volume1.Volume).ShouldBe(123 * 456 * 789);

            var volume2 = new Size3(222, 555, 999);
            TheResultingValue(volume2.Volume).ShouldBe(222 * 555 * 999);
        }
    }
}
