using NUnit.Framework;
using System;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_SlotIndex {
        public static Slot TestSlot = new Slot() {
            Day = Day.Monday ,
            StartTime = Time.CreateTime_24HourFormat(7 , 0) ,
            EndTime = Time.CreateTime_24HourFormat(8 , 0)

        };
        [Test]
        public void Test_SlotIndex_1() {
            var s = TestSlot;
            var expected = new SlotIndex(0 , 0 , 2);
            var actual = new SlotIndex(s.Day , s.StartTime , s.EndTime.Minus(s.StartTime));
            Console.WriteLine(expected.ToString());
            Console.WriteLine(actual.ToString());
            Assert.True(expected.Equals(actual));
        }

        [Test]
        public void Test_SlotIndex_2() {
            var s = new Slot() {
                Day = Day.Tuesday ,
                StartTime = Time.CreateTime_24HourFormat(7 , 30) ,
                EndTime = Time.CreateTime_24HourFormat(9 , 0)

            }; ;
            var expected = new SlotIndex(1 , 1 , 3);
            var actual = new SlotIndex(s.Day , s.StartTime , s.EndTime.Minus(s.StartTime));
            Console.WriteLine(expected.ToString());
            Console.WriteLine(actual.ToString());
            Assert.True(expected.Equals(actual));
        }

        [Test]
        public void Test_SlotIndex_IntersectionCheck_1() {
            var a = new SlotIndex(0 , 1 , 2);
            var b = new SlotIndex(0 , 1 , 2);
            Assert.True(a.IntersectWith(b));
        }

        [Test]
        public void Test_SlotIndex_IntersectionCheck_2() {
            var a = new SlotIndex(0 , 2 , 2);
            var b = new SlotIndex(0 , 1 , 2);
            Assert.True(a.IntersectWith(b));
        }
        [Test]
        public void Test_SlotIndex_IntersectionCheck_3() {
            var a = new SlotIndex(0 , 0 , 2);
            var b = new SlotIndex(0 , 1 , 2);
            Assert.True(a.IntersectWith(b));
        }

        [Test]
        public void Test_SlotIndex_IntersectionCheck_4() {
            var a = new SlotIndex(0 , 1 , 3);
            var b = new SlotIndex(0 , 1 , 2);
            Assert.True(a.IntersectWith(b));
        }

        [Test]
        public void Test_SlotIndex_IntersectionCheck_5() {
            var a = new SlotIndex(0 , 1 , 2);
            var b = new SlotIndex(0 , 1 , 3);
            Assert.True(a.IntersectWith(b));
        }

        [Test]
        public void Test_SlotIndex_IntersectionCheck_6() {
            var a = new SlotIndex(0 , 2 , 2);
            var b = new SlotIndex(1 , 1 , 2);
            Assert.False(a.IntersectWith(b));
        }

        [Test]
        public void Test_SlotIndex_IntersectionCheck_7() {
            var a = new SlotIndex(0 , 1 , 2);
            var b = new SlotIndex(0 , 3 , 2);
            Assert.False(a.IntersectWith(b));
        }

        [Test]
        public void Test_SlotIndex_IntersectionCheck_8() {
            var a = new SlotIndex(0 , 3 , 2);
            var b = new SlotIndex(1 , 1 , 2);
            Assert.False(a.IntersectWith(b));
        }
    }
}
