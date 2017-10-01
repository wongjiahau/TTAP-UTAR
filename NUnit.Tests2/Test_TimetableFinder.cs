using System;
using System.Collections.Generic;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.TokenParser;
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

        [Test]
        [Ignore("This test is slow to run")]
        public void FullTest_1() {
            var data = new HtmlSlotParser().Parse(Helper.RawStringOfTestFile("SampleData-FAM-2017-2ndSem.html"));
            var input = new List<Slot>();
            input.AddRange(data.FindAll(x => x.Code == "UKAF4023")); //ATP
            input.AddRange(data.FindAll(x => x.Code == "MPU34022")); //ACP
            input.AddRange(data.FindAll(x => x.Code == "MPU34032")); //CP
            input.AddRange(data.FindAll(x => x.Code == "UKAI3013")); //E
            var result = new TimetableFinder().GetPossibleTimetables(input.ToArray());
            Assert.AreEqual(112 , result.Count);
        }
    }
}
