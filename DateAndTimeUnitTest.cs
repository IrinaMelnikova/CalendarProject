using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimeAndDateUnitTest
{
    [TestClass]
    public class TimeAndDateTestClass
    {
        [TestMethod]
        public void deleteSymbolsInEmptyString()
        {
            // arrange
            string emptyString = "";
            string expected = "";

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            string actual = newDate.deleteSymbolsInIcalDate(emptyString);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void deleteSymbolsInStringWithOneSymbolT()
        {
            // arrange
            string stringWithOneSymbolT = "T";
            string expected = "";

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            string actual = newDate.deleteSymbolsInIcalDate(stringWithOneSymbolT);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void deleteSymbolsInStringWithOneSymbolZ()
        {
            // arrange
            string stringWithOneSymbolZ = "Z";
            string expected = "";

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            string actual = newDate.deleteSymbolsInIcalDate(stringWithOneSymbolZ);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void deleteSymbolsInStringWithoutTOrZ()
        {
            // arrange
            string stringWithoutTOrZ = "9305689G6";
            string expected = "9305689G6";

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            string actual = newDate.deleteSymbolsInIcalDate(stringWithoutTOrZ);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void deleteSymbolsInStringWithTInside()
        {
            // arrange
            string stringWithTInside = "9305689T6";
            string expected = "93056896";

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            string actual = newDate.deleteSymbolsInIcalDate(stringWithTInside);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void deleteSymbolsInStringWithZInside()
        {
            // arrange
            string stringWithTInside = "12Z34";
            string expected = "1234";

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            string actual = newDate.deleteSymbolsInIcalDate(stringWithTInside);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void deleteSymbolsInStringStartsWithT()
        {
            // arrange
            string stringStartsWithT = "T1234";
            string expected = "1234";

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            string actual = newDate.deleteSymbolsInIcalDate(stringStartsWithT);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void deleteSymbolsInStringStartsWithZ()
        {
            // arrange
            string stringStartsWithZ = "Z1234";
            string expected = "1234";

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            string actual = newDate.deleteSymbolsInIcalDate(stringStartsWithZ);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void deleteSymbolsInStringEndsWithZ()
        {
            // arrange
            string stringEndsWithZ = "1234Z";
            string expected = "1234";

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            string actual = newDate.deleteSymbolsInIcalDate(stringEndsWithZ);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void deleteSymbolsInStringEndsWithT()
        {
            // arrange
            string stringEndsWithT = "1234T";
            string expected = "1234";

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            string actual = newDate.deleteSymbolsInIcalDate(stringEndsWithT);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void formatingEmptyString()
        {
            // arrange
            string empryString = "";
            DateTime expected = DateTime.MinValue;

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            DateTime actual = newDate.format(empryString);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void formatingStringWithOneNumber()
        {
            // arrange
            string stringWithOneNumber = "1";
            DateTime expected = DateTime.MinValue;

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            DateTime actual = newDate.format(stringWithOneNumber);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void formatingStringWithNumbers()
        {
            // arrange
            string stringWithNumbers = "123456789";
            DateTime expected = DateTime.MinValue;

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            DateTime actual = newDate.format(stringWithNumbers);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void formatingStringWithLength14()
        {
            // arrange
            string stringWithLength14 = "12s34f56h78ah9";
            DateTime expected = DateTime.MinValue;

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            DateTime actual = newDate.format(stringWithLength14);
            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void formatingStringWithSymbols()
        {
            // arrange
            string stringWithSymbols = "12s34f56h78a";
            DateTime expected = DateTime.MinValue;

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            DateTime actual = newDate.format(stringWithSymbols);

            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void formatingStringDate()
        {
            // arrange
            string stringDate = "20161108000000";
            DateTime expected = new DateTime(2016, 08, 11, 00, 00, 00);

            // act
            TimeAndDate.TimeAndDate newDate = new TimeAndDate.TimeAndDate("20161108T000000Z");
            DateTime actual = newDate.format(stringDate);

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}
