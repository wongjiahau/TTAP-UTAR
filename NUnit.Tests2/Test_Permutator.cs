using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.TimetableFinder;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_Permutator {
        private List<Slot> input() {
            var result = new List<Slot>();
            result.AddRange(TestData.GetSlotsByName(TestData.Subjects.Hydrology));
            result.AddRange(TestData.GetSlotsByName(TestData.Subjects.StructuralAnalysisII));
            result.AddRange(TestData.GetSlotsByName(TestData.Subjects.HighwayAndTransportation));
            result.AddRange(TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsII));
            result.AddRange(TestData.GetSlotsByName(TestData.Subjects.IntroductionToBuildingServices));
            return result;
        }

        [Test]
        public void Test_TimetableFinder_UsingRunByConsideringWeekNumber() {
            var input = SubjectModel.Parse(this.input());
            int expectedCount = 616872;
            var timer = Stopwatch.StartNew();
            var result =
                new TimetableFinder().GetPossibleTimetables(input , Permutator.Run_v2_WithConsideringWeekNumber);
            timer.Stop();
            Console.WriteLine("Combination count : " + result.Count);
            Console.WriteLine("Elapsed time : " + timer.Elapsed.TotalSeconds + " s");
            Assert.True(result.Count == expectedCount);
        }

        [Test]
        public void Test_TimetableFinder_UsingRunByWithoutConsideringWeekNumber() {
            var input = SubjectModel.Parse(this.input());
            int expectedCount = 285696;
            var timer = Stopwatch.StartNew();
            var result =
                new TimetableFinder().GetPossibleTimetables(input , Permutator.Run_v2_withoutConsideringWeekNumber);
            timer.Stop();
            Console.WriteLine("Combination count : " + result.Count);
            Console.WriteLine("Elapsed time : " + timer.Elapsed.TotalSeconds + " s");
            Assert.True(result.Count == expectedCount);

        }
        [Test]
        public void Test_PermutateV4_Runv2_WithConsideringWeekNumber() {
            int expectedCount = 616872;
            var timer = Stopwatch.StartNew();
            IPermutator permutator = new Permutator_WithConsideringWeekNumber();
            var result = permutator.Permutate(input().ToArray());
            timer.Stop();
            Console.WriteLine("Combination count : " + result.Count);
            Console.WriteLine("Elapsed time : " + timer.Elapsed.TotalSeconds + " s");
            Assert.True(result.Count == expectedCount);
        }

        [Test]
        public void Test_PermutateV4_Runv2_WithoutConsideringWeekNumber() {
            int expectedCount = 285696;
            var timer = Stopwatch.StartNew();
            IPermutator permutator = new Permutator_WithoutConsideringWeekNumber();
            var result = permutator.Permutate(input().ToArray());
            timer.Stop();
            Console.WriteLine("Combination count : " + result.Count);
            Console.WriteLine("Elapsed time : " + timer.Elapsed.TotalSeconds + " s");
            Assert.True(result.Count == expectedCount);
        }

        [Test]
        public void Test_Benchmarking_ListToArray() {
            //The purpose of this test is to identifiy whethere List.ToArray() is a slowing factor for TimetableFinder
            var input = this.input();
            int loopCount = 1000; //Actually this is more than enough, since TimetableFinder is expected to call List.ToArray() less than a hundred time
            Console.WriteLine("List size is " + input.Count);
            Console.WriteLine(@"Calling List.ToArray() by " + loopCount + @" times");
            var timer = Stopwatch.StartNew();
            for (int i = 0 ; i < loopCount; i++) {
                var x = input.ToArray();
            }
            timer.Stop();
            Console.WriteLine("Elapsed time : " + timer.Elapsed.TotalSeconds + " s");
            Console.WriteLine(timer.Elapsed.TotalSeconds < 0.01
                ? "Conclusion : List.ToArray() is NOT a slowing factor."
                : "Conclusion : List.ToArray() is a slowing factor.");
        }


    }
}
