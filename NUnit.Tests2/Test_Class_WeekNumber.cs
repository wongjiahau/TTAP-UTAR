using NUnit.Framework;
using System;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Class.Converter;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_Class_WeekNumber {
        [Test]
        public void Test_WeekNumber_ClashesWith_1() {
            var input1 = new WeekNumber(new List<int> { 1 , 2 , 3 , 4 , 5 });
            var input2 = new WeekNumber(new List<int> { 5 , 6 , 7 , 8 , 9 });
            Assert.True(input1.IntersectWith(input2));
        }

        [Test]
        public void Test_WeekNumber_ClashesWith_2() {
            var input1 = new WeekNumber(new List<int> { 1 , 3 , 5 , 7 , 9 });
            var input2 = new WeekNumber(new List<int> { 2 , 4 , 6 , 8 , 10 });
            Console.WriteLine(Convert.ToString(input1._weekNumberInBinary , 2));
            Console.WriteLine(Convert.ToString(input2._weekNumberInBinary , 2));
            Assert.False(input1.IntersectWith(input2));
        }

        [Test]
        public void Test_WeekNumber_ClashesWith_3() {
            var input1 = new WeekNumber(new List<int> { 6 , 8 });
            var input2 = new WeekNumber(new List<int> { 4 , 3 });
            Assert.False(input1.IntersectWith(input2));
        }

        [Test]
        public void Test_WeekNumber_ClashesWith_4() {
            var input1 = new WeekNumber(new List<int> { 1 , 3 , 7 , 9 , 11 , 13 });
            var input2 = new WeekNumber(new List<int> { 1 , 2 , 3 , 4 , 5 , 6 , 7 , 8 , 9 , 10 , 11 , 12 , 13 , 14 });
            Assert.True(input1.IntersectWith(input2));
        }

        [Test]
        public void Test_WeekNumber_Parse_1() {
            var input = "3,5,7,9,11,13";
            var expected = new WeekNumber(new List<int> { 3 , 5 , 7 , 9 , 11 , 13 });
            var actual = WeekNumber.Parse(input);
            Assert.True(expected.Equals(actual));
        }

        [Test]
        public void Test_WeekNumber_Parse_2() {
            var input = "1-5";
            var expected = new WeekNumber(new List<int> { 1 , 2 , 3 , 4 , 5 });
            var actual = WeekNumber.Parse(input);
            Assert.True(expected.Equals(actual));
        }

        [Test]
        public void Test_WeekNumber_Parse_3() {
            var input = "1-3,7-9";
            var expected = new WeekNumber(new List<int> { 1 , 2 , 3 , 7 , 8 , 9 });
            var actual = WeekNumber.Parse(input);
            Assert.True(expected.Equals(actual));
        }

        [Test]
        public void Test_WeekNumber_Parse_4() {
            var input = "1-3,7-9,11,13";
            var expected = new WeekNumber(new List<int> { 1 , 2 , 3 , 7 , 8 , 9 , 11 , 13 });
            var actual = WeekNumber.Parse(input);
            Assert.True(expected.Equals(actual));
        }

        [Test]
        public void Test_WeekNumber_ToString_0() {
            var x = new WeekNumber(new List<int> { 1 , 3 , 5 });
            Assert.AreEqual("1,3,5" , x.ToString());
        }

        [Test]
        public void Test_WeekNumber_ToString_1() {
            var x = new WeekNumber(new List<int> { 1 , 2 , 3 , 4 , 5 });
            Assert.AreEqual(x.ToString() , "1-5");
        }

        [Test]
        public void Test_WeekNumber_ToString_2() {
            var x = new WeekNumber(new List<int> { 1 , 2 , 4 , 5 , 6 });
            Assert.AreEqual(x.ToString() , "1-2,4-6");
        }

        [Test]
        public void Test_WeekNumber_ToString_3() {
            var x = new WeekNumber(new List<int> { 1 , 4 , 5 , 6 });
            Assert.AreEqual(x.ToString() , "1,4-6");
        }

        [Test]
        public void Test_WeekNumber_ToString_4() {
            var x = new WeekNumber(new List<int> { 1 , 4 , 5 , 6 , 8 , 9 , 10 });
            Assert.AreEqual(x.ToString() , "1,4-6,8-10");
        }
    }
}