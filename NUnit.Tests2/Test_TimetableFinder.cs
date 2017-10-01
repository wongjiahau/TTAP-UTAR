using System;
using System.Collections.Generic;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.TimetableFinder;

namespace NUnit.Tests2 {
    [TestFixture]
    public class TestTimetableFinder {
        private List<Slot> GetInput() {
            var input = TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsII);
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.HighwayAndTransportation));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.Hydrology));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.IntroductionToBuildingServices));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.StructuralAnalysisII));
            return input;
        }



        [Test]
        public void Test_TimetableFinder_SortBySlotCount_1() {
            var input = GetInput();
            var subjects = SubjectModel.Parse(input);
            var timtableFinder = new TimetableFinder();
            var result = timtableFinder.SortBySlotCount(subjects);
            for (int i = 0 ; i < result.Count ; i++) {
                Console.WriteLine(result[i].Code + " " + result[i].Slots.Count);
            }
        }

        [Test]
        public void Test_Correctness_1() {
            var input = new List<Slot>();
            input.AddRange(TestData.Default().FindAll(x => x.UID == 28)); //Lecture of BEAM
            input.AddRange(TestData.Default().FindAll(x => x.UID == 6));//Lecture of BKA
            var result = new TimetableFinder().GetPossibleTimetables(input.ToArray());
            Assert.AreEqual(null , result);
        }
 
        [Test]
        public void Test_Correctness_2() {
            var input = new List<Slot>();
            input.AddRange(TestData.Default().FindAll(x => x.UID == 29)); //L2 of BEAM
            input.AddRange(TestData.Default().FindAll(x => x.UID == 33)); //T4 of BEAM
            input.AddRange(TestData.Default().FindAll(x => x.UID == 6));//L1 of BKA
            var result = new TimetableFinder().GetPossibleTimetables(input.ToArray());
            Assert.AreEqual(null , result);
        }
    }
}
