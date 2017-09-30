using System;
using System.Collections.Generic;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.TimetableFinder;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_TimetableFinder {
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
            var subjects = SubjectModel.Parse(input);
            var timtableFinder = new TimetableFinder();
            var result = timtableFinder.GetPossibleTimetables(subjects , Permutator.Run_v2_withoutConsideringWeekNumber);
            Assert.AreEqual(0 , result.Count);
        }

        [Test]
        public void Test_Correctness_2() {
            var input = new List<Slot>();
            input.AddRange(TestData.Default().FindAll(x => x.UID == 28)); //Lecture of BEAM
            input.AddRange(TestData.Default().FindAll(x => x.UID == 6));//Lecture of BKA
            var subjects = SubjectModel.Parse(input);
            var timtableFinder = new TimetableFinder();
            var result = timtableFinder.GetPossibleTimetables(subjects , Permutator.Run_v2_withoutConsideringWeekNumber);
            if (result.Count == 0) Assert.Pass();
            Assert.AreEqual(4 , result[0].Count);
        }
    }
}
