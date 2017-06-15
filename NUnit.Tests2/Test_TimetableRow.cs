using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_TimetableRow {
        [Test]
        public void Test_TimetableRow_Add() {
            var input1 = new Slot();
            input1.StartTime = Time.CreateTime_24HourFormat(8 , 0);
            input1.EndTime = Time.CreateTime_24HourFormat(10 , 0);
            input1.WeekNumber = WeekNumber.Parse("1-14");

            var input2 = new Slot();
            input2.StartTime = Time.CreateTime_24HourFormat(8 , 0);
            input2.EndTime = Time.CreateTime_24HourFormat(9 , 0);
            input2.WeekNumber = WeekNumber.Parse("2,5");
            var ttr = new TimetableRow();
            ttr.Add(input1);
            Assert.False(ttr.Add(input2));

        }

        [Test]
        public void Test_TimetableRow_IntersectWith_1() {
            var ttr1 = new TimetableRow();
            ttr1.Add(TestData.GetSlot(3));
            var ttr2 = new TimetableRow();
            ttr2.Add(TestData.GetSlot(11));
            Assert.IsTrue(ttr1.IntersectWith(ttr2));

        }

        [Test]
        public void Test_TimetableRow_IntersectWith_2() {
            var ttr1 = new TimetableRow();
            ttr1.Add(TestData.GetSlot(3));
            var ttr2 = new TimetableRow();
            ttr2.Add(TestData.GetSlot(25));
            Assert.IsFalse(ttr1.IntersectWith(ttr2));

        }
    }
}
