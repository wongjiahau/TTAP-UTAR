using NUnit.Framework;
using System.Collections.Generic;
using Time_Table_Arranging_Program;


namespace NUnit.Tests1 {
    [TestFixture]
    public class TestClass {
        private static Slot testSlot1 = new Slot("math" , "1" , "Lecture" , "Monday" , new Time(3 , 0) , new Time(4 , 0));
        private static Slot testSlot2 = new Slot("math" , "2" , "Lecture" , "Monday" , new Time(3 , 0) , new Time(4 , 0));
        private static List<Slot> testSlots = new List<Slot>() { testSlot1 , testSlot2 };
        private static List<List<Slot>> Expected = new List<List<Slot>>()
        {
           new List<Slot> { new Slot("math", "1", "Lecture", "Monday", new Time(3, 0), new Time(4, 0))},
          new List<Slot>  { new Slot("math" , "2" , "Lecture" , "Monday" , new Time(3 , 0) , new Time(4 , 0))}
        };
        [Test]
        public void TestPermutator_1() {
            //  var p = new Permutator(testSlots);
            // var actual = p.Permutate();

            // Assert.AreEqual(Expected , actual); ;
        }

        [Test]
        public void TestTimeClass() {
            var time1 = new Time(8 , 45);
            var time2 = new Time(0 , 15);
            var time3 = time1 + time2;
            var expected = new Time(9 , 0);
            Assert.True(expected == time3);
        }

        [Test]
        public void TestTimeClass2() {
            var time1 = new Time(8 , 0);
            var time2 = new Time(0 , 35);
            var time3 = time1 - time2;
            var expected = new Time(7 , 25);
            Assert.True(expected == time3);
        }

        [Test]
        public void testHashSet() {
            var set1 = new HashSet<int>();
            var set2 = new HashSet<int>();
            set1.Add(1);
            set1.Add(2);
            set2.Add(2);
            set2.Add(1);
            //set2.Add(2);
            var set3 = new HashSet<HashSet<int>>();
            set3.Add(set1);
            set3.Add(set2);
            Assert.True(set1.SetEquals(set2));
        }

    }
}
