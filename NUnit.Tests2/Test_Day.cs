using System;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class.Converter;

namespace NUnit.Tests2 {
    [TestFixture]
   public class Test_Day {
        private string ToBinString(Day x) {
            return Convert.ToString(x.ToBinary(), 2);
        }
        [Test]
        public void Test_Day_ToBinary_1() {
            var input = Day.Monday;
            Assert .IsTrue(ToBinString(input) == "1");            
        }

        [Test]
        public void Test_Day_ToBinary_2() {
            var input = Day.Tuesday;
            Assert.IsTrue(ToBinString(input) == "10");
        }

        [Test]
        public void Test_Day_ToBinary_3() {
            var input = Day.Wednesday;
            Assert.IsTrue(ToBinString(input) == "100");
        }

        [Test]
        public void Test_Day_ToBinary_4() {
            var input = Day.Thursday;
            Assert.IsTrue(ToBinString(input) == "1000");
        }

        [Test]
        public void Test_Day_ToBinary_5() {
            var input = Day.Friday;
            Assert.IsTrue(ToBinString(input) == "10000");
        }

        [Test]
        public void Test_Day_ToBinary_6() {
            var input = Day.Saturday;
            Assert.IsTrue(ToBinString(input) == "100000");
        }

        [Test]
        public void Test_Day_ToBinary_7() {
            var input = Day.Sunday;
            Assert.IsTrue(ToBinString(input) == "1000000");
        }

        [Test]
        public void Test_Day_IntersectWith() {
            var rand1= new Random();
            var rand2 = new Random();
            for (int i = 0; i < 100000; i++) {
                var day1 = Day.GetDay(rand1.Next(1, 7));
                var day2 = Day.GetDay(rand2.Next(1, 7));
                if (day1.Equals(day2)) {
                    Assert.IsTrue((day1.ToBinary() & day2.ToBinary()) > 0);
                }
            }
        }
    }
}
