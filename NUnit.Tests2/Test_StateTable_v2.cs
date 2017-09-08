using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.StateSummary;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_StateTable_v2 {
        [Test]
        public void Test_StateTable_ParseString_1() {
            string input =
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000";
            var expected = new int[7] { 0 , 0 , 0 , 0 , 0 , 0 , 0 };
            var actual = StateTable.ParseString_AsStateOfDefinitelyOccupied(input);
            Assert.IsTrue(Enumerable.SequenceEqual(actual , expected));
        }

        [Test]
        public void Test_StateTable_ParseString_2() {
            string input =
                "10000000000000000000000000000000~" +
                "10000000000000000000000000000000~" +
                "10000000000000000000000000000000~" +
                "10000000000000000000000000000000~" +
                "10000000000000000000000000000000~" +
                "10000000000000000000000000000000~" +
                "10000000000000000000000000000000";
            var expected = new int[7] { 1 , 1 , 1 , 1 , 1 , 1 , 1 };
            var actual = StateTable.ParseString_AsStateOfDefinitelyOccupied(input);
            Assert.IsTrue(Enumerable.SequenceEqual(actual , expected));
        }

        [Test]
        public void Test_StateTable_GetStateOfDefinitelyOccupied_1() {
            var input = Permutator.Run_v2_withoutConsideringWeekNumber(TestData
                .GetSlotsByName(TestData.Subjects.AdvancedStructuralSteelDesign).ToArray());
            var expected = StateTable.ParseString_AsStateOfDefinitelyOccupied(
                "00000000000000111100000000000000~" +
                "00110000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000"
            );
            var actual = StateTable.GetStateOfDefinitelyOccupied(input);
            Assert.IsTrue(Enumerable.SequenceEqual(actual , expected));
        }

        [Test]
        public void Test_StateTable_GetStateOfDefinitelyOccupied_2() {
            var input = Permutator.Run_v2_withoutConsideringWeekNumber(TestData
                .GetSlotsByName(TestData.Subjects.HighwayAndTransportation).ToArray());
            var expected = StateTable.ParseString_AsStateOfDefinitelyOccupied(
                "00000000000000000000001100000000~" +
                "00000000000000000000000000000000~" +
                "00000011110000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000"
            );
            var actual = StateTable.GetStateOfDefinitelyOccupied(input);
            for (int i = 0; i < 7; i++) {
                if (expected[i] != actual[i]) {
                    string errorMessage="Error at day = " + i + "\n";
                    errorMessage += "Expected = " + Convert.ToString(expected[i], 2) + "\n";
                    errorMessage += "Actual   = " + Convert.ToString(actual[i], 2);
                    Assert.Fail(errorMessage);
                }
            }
            Assert.Pass();
        }

        [Test]
        public void Test_StateTable_Filter_1() {
            var input = TestData.GetSlotsByName(TestData.Subjects.AdvancedStructuralSteelDesign);
            var inputState = StateTable.ParseString_AsStateOfDefinitelyOccupied(
                "11111111111111111111111111111111~" +
                "11111111111111111111111111111111~" +
                "11111111111111111111111111111111~" +
                "11111111111111111111111111111111~" +
                "11111111111111111111111111111111~" +
                "11111111111111111111111111111111~" +
                "11111111111111111111111111111111"
            );
            var expectedCount = 0;
            var actualCount = StateTable.Filter(input, inputState).Count;
            Assert.AreEqual(expectedCount, actualCount);
        }
 
        [Test]
        public void Test_StateTable_Filter_2() {
            var input = TestData.GetSlotsByName(TestData.Subjects.AdvancedStructuralSteelDesign);
            var inputState = StateTable.ParseString_AsStateOfDefinitelyOccupied(
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000"
            );
            var expectedCount = input.Count;
            var actualCount = StateTable.Filter(input, inputState).Count;
            Assert.AreEqual(expectedCount, actualCount);
        }
  
        [Test]
        public void Test_StateTable_Filter_3() {
            var input = TestData.GetSlotsByName(TestData.Subjects.AdvancedStructuralSteelDesign);
            var inputState = StateTable.ParseString_AsStateOfDefinitelyOccupied(
                "00000000000000000000000000000000~" +
                "00000011000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000"
            );
            var expectedCount = input.Count - 2;
            var actualCount = StateTable.Filter(input, inputState).Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void Test_StateTable_Filter_ShouldRemoveRelatedSlots() {
            var input = new List<Slot> {TestData.GetSlot(309), TestData.GetSlot(311)};
            var inputState = StateTable.ParseString_AsStateOfDefinitelyOccupied(
                "00000011110000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000"
            );
            var expectedCount = 0;
            var actualCount = StateTable.Filter(input, inputState).Count;
            Assert.AreEqual(expectedCount, actualCount);

        }
    }
}
