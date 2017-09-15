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
            int expectedCount = 616872;
            List<List<Slot>> result = new List<List<Slot>>();
            Benchmark(() => {
                var input = SubjectModel.Parse(this.input());
                result =
                    new TimetableFinder().GetPossibleTimetables(input , Permutator.Run_v2_WithConsideringWeekNumber);
            }, "TimetableFinder_ConsideringWeekNumber");
            Assert.True(result.Count == expectedCount, $"Expected count is {expectedCount} but actual is {result.Count}");
        }

        [Test]
        public void Test_TimetableFinder_UsingRunByWithoutConsideringWeekNumber() {
            int expectedCount = 285696;
            List<List<Slot>> result = new List<List<Slot>>();
            Benchmark(() => {
                var input = SubjectModel.Parse(this.input());
                result =
                    new TimetableFinder().GetPossibleTimetables(input , Permutator.Run_v2_withoutConsideringWeekNumber);
            } , "TimetableFinder_WithoutConsideringWeekNumber");
            Assert.True(result.Count == expectedCount);

        }
        [Test]
        public void Test_PermutateV4_Runv2_WithConsideringWeekNumber() {
            int expectedCount = 616872;
            IPermutator permutator = new Permutator_WithConsideringWeekNumber();
            List<List<Slot>> result = new List<List<Slot>>();
            Benchmark(() => {
                result = permutator.Permutate(input().ToArray());
            } , "Runv2_WithConsideringWeekNumber");
            Assert.True(result.Count == expectedCount);
        }

        [Test]
        public void Test_PermutateV4_Runv2_WithoutConsideringWeekNumber() {
            int expectedCount = 285696;
            IPermutator permutator = new Permutator_WithoutConsideringWeekNumber();
            List<List<Slot>> result = new List<List<Slot>>();
            Benchmark(() => {
                result = permutator.Permutate(input().ToArray());
            } , "Runv2_WithoutConsideringWeekNumber");
            Assert.True(result.Count == expectedCount);
        }

        [Test]
        public void Benchmarkihg_ListToArray() {
            //The purpose of this test is to identifiy whethere List.ToArray() is a slowing factor for TimetableFinder
            var input = this.input();
            int loopCount = 1000; //Actually this is more than enough, since TimetableFinder is expected to call List.ToArray() less than a hundred time
            Console.WriteLine("List size is " + input.Count);
            Console.WriteLine(@"Calling List.ToArray() by " + loopCount + @" times");
            Benchmark(() => {
                for (int i = 0 ; i < loopCount ; i++) {
                    var x = input.ToArray();
                }
            } , "List.ToArray()");
        }

        [Test]
        public void Benchmarking_SubjectModel_Parse() {
            Benchmark(() => {
                var result = SubjectModel.Parse(input());
            }, "SubjectModel.Parse()");
        }
        private static void Benchmark(Action act , string methodName , int iterations = 1) {
            GC.Collect();
            act.Invoke(); // run once outside of loop to avoid initialization costs
            Stopwatch timer = Stopwatch.StartNew();
            for (int i = 0 ; i < iterations ; i++) {
                act.Invoke();
            }
            timer.Stop();
            Console.WriteLine(methodName + " : " + timer.Elapsed.TotalSeconds / iterations + " s");
        }

    }
}
