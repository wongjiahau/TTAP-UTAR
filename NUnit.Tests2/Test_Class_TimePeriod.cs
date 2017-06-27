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
    }
}
