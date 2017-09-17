using System;
using System.Collections.Generic;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using static NUnit.Tests2.TestData;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_StateTable {
        [Test]
        public void Test_StateTable_1() {
            var stateTable = new StateTable_v1();
            var data = new List<Slot>();
            data.AddRange(GetSlotsByName(Subjects.Hydrology));
            data.AddRange(GetSlotsByName(Subjects.IntroductionToBuildingServices));
            data.AddRange(GetSlotsByName(Subjects.HighwayAndTransportation));
            data.AddRange(GetSlotsByName(Subjects.StructuralAnalysisII));

            var result = Permutator.Run_v2_WithConsideringWeekNumber(data.ToArray())[0];
            for (int i = 0 ; i < result.Count ; i++) {
                stateTable.Add(new IndexedSlot(result[i]));
            }
            Console.WriteLine(stateTable.ToString());
            Assert.Pass();
        }

        [Test]
        public void Test_DrawingSummaryState() {
            var stateTable = new StateTable_v1();
            var data = new List<Slot>();
            data.AddRange(GetSlotsByName(Subjects.Hydrology));
            data.AddRange(GetSlotsByName(Subjects.IntroductionToBuildingServices));
            data.AddRange(GetSlotsByName(Subjects.HighwayAndTransportation));
            data.AddRange(GetSlotsByName(Subjects.StructuralAnalysisII));
            var result = Permutator.Run_v2_WithConsideringWeekNumber(data.ToArray());
            for (int i = 0 ; i < result.Count ; i++) {
                for (int j = 0 ; j < result[i].Count ; j++) {
                    stateTable.Add(new IndexedSlot(result[i][j]));
                }
            }
            Console.WriteLine(stateTable.Draw(result.Count));
            Assert.Pass();
        }
        [Test]
        public void GenerateEnumForTestData() {
            var result = "";
            string previousName = "null";
            int firstId;
            foreach (var slot in TestSlots) {
                if (slot.SubjectName == previousName) continue;
                firstId = slot.UID;
                result += $"{firstId - 2}); \n";
                result += "case " + slot.SubjectName.Replace(" " , "") + ":" + "\n" +
                          $"return GetSlotRange({firstId},";

                previousName = slot.SubjectName;

            }
            Console.WriteLine(result);
            Assert.Pass();
        }
//Todo : Add test for the new StateTable generator
        [Test]
        public void SandboxTesting() {



        }
    }
}
