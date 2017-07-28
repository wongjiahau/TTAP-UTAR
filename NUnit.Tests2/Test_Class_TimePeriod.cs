using System;
using System.Linq;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class.Converter;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_Class_TimePeriod {
        [Test]
        public void Test_timePeriod_IntersectWith_1() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0));
            var input2 = new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0));
            Assert.True(input1.IntersectWith(input2));
        }

        [Test]
        public void Test_TimePeriod_IntersectWith_2() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0));
            var input2 = new TimePeriod(Time.CreateTime_24HourFormat(7 , 0) , Time.CreateTime_24HourFormat(9 , 0));
            Assert.True(input1.IntersectWith(input2));
        }

        [Test]
        public void Test_TimePeriod_IntersectWith_3() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0));
            var input2 = new TimePeriod(Time.CreateTime_24HourFormat(9 , 0) , Time.CreateTime_24HourFormat(11 , 0));
            Assert.True(input1.IntersectWith(input2));
        }

        [Test]
        public void Test_TimePeriod_IntersectWith_4() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0));
            var input2 = new TimePeriod(Time.CreateTime_24HourFormat(7 , 0) , Time.CreateTime_24HourFormat(11 , 0));
            Assert.True(input1.IntersectWith(input2));
        }

        [Test]
        public void Test_TimePeriod_IntersectWith_5() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0));
            var input2 = new TimePeriod(Time.CreateTime_24HourFormat(8 , 30) , Time.CreateTime_24HourFormat(9 , 30));
            Assert.True(input1.IntersectWith(input2));
        }

        [Test]
        public void Test_TimePeriod_Intersection_6() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0));
            var input2 = new TimePeriod(Time.CreateTime_24HourFormat(9 , 00) , Time.CreateTime_24HourFormat(11 , 0));
            Assert.True(input1.IntersectWith(input2));
        }

        [Test]
        public void Test_TimePeriod_ToBinary_1() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0));
            var result = Convert.ToString(input1.ToBinary(),2);
            Assert.AreEqual("111100", result);            
        }

        [Test]
        public void Test_TimePeriod_ToBinary_2() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(11 , 0) , Time.CreateTime_24HourFormat(13 , 0));
            var result = Convert.ToString(input1.ToBinary() , 2);
            Console.WriteLine("Result is " +result);
            Assert.AreEqual("000000001111".Reverse() , result);
        }

        [Test]
        public void Test_TimePeriod_ToBinary_3() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(18 , 0) , Time.CreateTime_24HourFormat(20 , 0));
            var result = Convert.ToString(input1.ToBinary() , 2);
            Console.WriteLine("Result is " + result);
            Assert.AreEqual("00000000000000000000001111".Reverse() , result);
        }

        [Test]
        public void Test_TimePeriod_ToBinary_4() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(21 , 0) , Time.CreateTime_24HourFormat(23 , 0));
            var result = Convert.ToString(input1.ToBinary() , 2);
            Console.WriteLine("Result is " + result);
            Assert.AreEqual("00000000000000000000000000001111".Reverse() , result);
        }

        [Test]
        public void Test_TimePeriod_ToBinary_5() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(8 , 30) , Time.CreateTime_24HourFormat(10 , 0));
            var result = Convert.ToString(input1.ToBinary() , 2);
            Console.WriteLine("Result is " + result);
            Assert.AreEqual("000111".Reverse() , result);
        }

        [Test]
        public void Test_TimePeriod_ANDing() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(8 , 30) , Time.CreateTime_24HourFormat(10 , 0)).ToBinary();
            int originalValue = Convert.ToInt32("11111111111111111111111111111111", 2);
            originalValue &= input1;
            Assert.AreEqual(originalValue , input1);

        }

        [Test]
        public void Test_TimePeriod_ORing() {
            var input1 = new TimePeriod(Time.CreateTime_24HourFormat(8 , 30) , Time.CreateTime_24HourFormat(10 , 0)).ToBinary();
            int originalValue = Convert.ToInt32("0" , 2);
            originalValue |= input1;
            Assert.AreEqual(originalValue , input1);
        }

        [Test]
        public void Test_TimePeriod_Parse_1() {
            var input = "09:00 AM - 12:00 PM";
            var expected = new TimePeriod(Time.CreateTime_24HourFormat(9,0), Time.CreateTime_24HourFormat(12,0));
            var actual = TimePeriod.Parse(input);
            Assert.IsTrue(expected.Equals(actual));
        }

        [Test]
        public void Test_TimePeriod_Parse_2() {
            var input = "08:30 AM - 1:00 PM";
            var expected = new TimePeriod(Time.CreateTime_24HourFormat(8 , 30) , Time.CreateTime_24HourFormat(13 , 0));
            var actual = TimePeriod.Parse(input);
            Assert.IsTrue(expected.Equals(actual));
        }

    }
}
