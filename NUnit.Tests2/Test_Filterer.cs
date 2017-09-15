using NUnit.Framework;
using System;
using System.Collections.Generic;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_Filterer {
        private static readonly List<Predicate<Slot>> Predicates_AllDay = new List<Predicate<Slot>>() { x => x.Day == Day.Monday };
        private static readonly List<Predicate<Slot>> Predicates_Before = new List<Predicate<Slot>>() { x => x.StartTime.LessThan(Time.CreateTime_24HourFormat(8 , 0)) };
        [Test]
        public void Test_Predicates_AllDay_1() {
            var input = TestData.GetSlotRange(309 , 315).ToArray();
            Slot[] result = Filterer.Filter(input , Predicates_AllDay);
            Assert.True(result == null);

        }

        [Test]
        public void Test_Predicates_AllDay_2() {

            var input = new Slot[] { TestData.GetSlot(281) , TestData.GetSlot(289) };
            Slot[] result = Filterer.Filter(input , Predicates_AllDay);
            Assert.True(result == null);

        }

        [Test]
        public void Test_Predicates_AllDay_3() {
            var input = TestData.GetSlotRange(281 , 307).ToArray();
            Slot[] result = Filterer.Filter(input , Predicates_AllDay);
            Assert.True(result.Length == 11);

        }

        [Test]
        public void Test_Predicates_Before_1() {

            var input = new Slot[] { TestData.GetSlot(9) };
            Slot[] result = Filterer.Filter(input , Predicates_Before);
            Assert.True(result.Length == 1);

        }
    }
}
