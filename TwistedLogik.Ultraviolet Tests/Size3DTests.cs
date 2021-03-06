﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class Size3DTests : UltravioletTestFramework
    {
        [TestMethod]
        public void Size3D_IsConstructedProperly()
        {
            var result = new Size3D(123.45, 456.78, 789.99);

            TheResultingValue(result)
                .ShouldBe(123.45, 456.78, 789.99);
        }

        [TestMethod]
        public void Size3D_OpEquality()
        {
            var volume1 = new Size3D(123.45, 456.78, 789.99);
            var volume2 = new Size3D(123.45, 456.78, 789.99);
            var volume3 = new Size3D(123.45, 555, 789.99);
            var volume4 = new Size3D(222, 456.78, 789.99);
            var volume5 = new Size3D(123.45, 456.78, 999);

            TheResultingValue(volume1 == volume2).ShouldBe(true);
            TheResultingValue(volume1 == volume3).ShouldBe(false);
            TheResultingValue(volume1 == volume4).ShouldBe(false);
            TheResultingValue(volume1 == volume5).ShouldBe(false);
        }

        [TestMethod]
        public void Size3D_OpInequality()
        {
            var volume1 = new Size3D(123.45, 456.78, 789.99);
            var volume2 = new Size3D(123.45, 456.78, 789.99);
            var volume3 = new Size3D(123.45, 555, 789.99);
            var volume4 = new Size3D(222, 456.78, 789.99);
            var volume5 = new Size3D(123.45, 456.78, 999);

            TheResultingValue(volume1 != volume2).ShouldBe(false);
            TheResultingValue(volume1 != volume3).ShouldBe(true);
            TheResultingValue(volume1 != volume4).ShouldBe(true);
            TheResultingValue(volume1 != volume5).ShouldBe(true);
        }

        [TestMethod]
        public void Size3D_EqualsObject()
        {
            var volume1 = new Size3D(123.45, 456.78, 789.99);
            var volume2 = new Size3D(123.45, 456.78, 789.99);

            TheResultingValue(volume1.Equals((Object)volume2)).ShouldBe(true);
            TheResultingValue(volume1.Equals("This is a test")).ShouldBe(false);
        }

        [TestMethod]
        public void Size3D_EqualsSize3D()
        {
            var volume1 = new Size3D(123.45, 456.78, 789.99);
            var volume2 = new Size3D(123.45, 456.78, 789.99);
            var volume3 = new Size3D(123.45, 555, 789.99);
            var volume4 = new Size3D(222, 456.78, 789.99);
            var volume5 = new Size3D(123.45, 456.78, 999);

            TheResultingValue(volume1.Equals(volume2)).ShouldBe(true);
            TheResultingValue(volume1.Equals(volume3)).ShouldBe(false);
            TheResultingValue(volume1.Equals(volume4)).ShouldBe(false);
            TheResultingValue(volume1.Equals(volume5)).ShouldBe(false);
        }

        [TestMethod]
        public void Size3D_TryParse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 789.99";
            var result = default(Size3D);
            if (!Size3D.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Size3D.");

            TheResultingValue(result)
                .ShouldBe(123.45, 456.78, 789.99);
        }

        [TestMethod]
        public void Size3D_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Size3D);
            var succeeded = Size3D.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [TestMethod]
        public void Size3D_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 789.99";
            var result = Size3D.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123.45, 456.78, 789.99);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Size3D_Parse_FailsForInvalidStrings()
        {
            Size3D.Parse("foo");
        }

        [TestMethod]
        public void Size3D_Parse_CanRoundTrip()
        {
            var size1 = Size3D.Parse("123.4 456.7 789.1");
            var size2 = Size3D.Parse(size1.ToString());

            TheResultingValue(size1 == size2).ShouldBe(true);
        }

        [TestMethod]
        public void Size3D_Volume_IsCalculatedCorrectly()
        {
            var volume1width  = 123.45;
            var volume1height = 456.78;
            var volume1depth  = 789.99;
            var volume1 = new Size3D(volume1width, volume1height, volume1depth);
            TheResultingValue(volume1.Volume).ShouldBe(volume1width * volume1height * volume1depth);

            var volume2width  = 222.22;
            var volume2height = 555.55;
            var volume2depth  = 999.99;
            var volume2       = new Size3D(volume2width, volume2height, volume2depth);
            TheResultingValue(volume2.Volume).ShouldBe(volume2width * volume2height * volume2depth);
        }
    }
}
