using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Model;

namespace NUnit.Tests2 {
    [TestFixture]
    public class TestClass_PermutateV4 {
        [Test]
        [Ignore("This test is being run on another test class already")]
        public void Test_PermutateV4_Runv2_WithConsideringWeekNumber() {
            int expectedCount = 616872;
            var input = new List<Slot>();
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.Hydrology));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.StructuralAnalysisII));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.HighwayAndTransportation));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsII));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.IntroductionToBuildingServices));
            var timer = Stopwatch.StartNew();
            var result = Permutator.Run_v2_WithConsideringWeekNumber(input.ToArray());
            timer.Stop();
            Console.WriteLine("Combination count : " + result.Count);
            Console.WriteLine("Elapsed time : " + timer.Elapsed.TotalSeconds + " s");
            Assert.True(result.Count == expectedCount);
        }

        [Test]
        [Ignore("Not testing this as BinarizedSlot is not used in production code")]
        public void Test_PermutateV4_Runv2_WithConsideringWeekNumber_ForBinarizedSlot() {
            int expectedCount = 616872;
            var raw = new List<Slot>();
            raw.AddRange(TestData.GetSlotsByName(TestData.Subjects.Hydrology));
            raw.AddRange(TestData.GetSlotsByName(TestData.Subjects.StructuralAnalysisII));
            raw.AddRange(TestData.GetSlotsByName(TestData.Subjects.HighwayAndTransportation));
            raw.AddRange(TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsII));
            raw.AddRange(TestData.GetSlotsByName(TestData.Subjects.IntroductionToBuildingServices));
            var input = new List<BinarizedSlotModel>();
            for (int i = 0 ; i < raw.Count ; i++) {
                input.Add(new BinarizedSlotModel(raw[i]));
            }
            var timer = Stopwatch.StartNew();
            var result = new List<List<BinarizedSlotModel>>();
            int loopCount = 5;
            for (int i = 0 ; i < loopCount ; i++) {
                result = PermutatorForBinarizedSlot.Run_v2_WithConsideringWeekNumber(input.ToArray());
            }
            timer.Stop();
            double averageElapsedSeconds = timer.Elapsed.TotalSeconds / loopCount;
            Console.WriteLine("Combination count : " + result.Count);
            Console.WriteLine("Elapsed time : " + averageElapsedSeconds + " s");
            Assert.True(result.Count == expectedCount);
        }

        [Test]
        [Ignore("This test is being run on another test class already")]
        public void Test_PermutateV4_Runv2_WithoutConsideringWeekNumber() {
            int expectedCount = 285696;
            var input = new List<Slot>();
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.Hydrology));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.StructuralAnalysisII));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.HighwayAndTransportation));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsII));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.IntroductionToBuildingServices));
            var timer = Stopwatch.StartNew();
            var result = Permutator.Run_v2_withoutConsideringWeekNumber(input.ToArray());
            timer.Stop();
            Console.WriteLine("Combination count : " + result.Count);
            Console.WriteLine("Elapsed time : " + timer.Elapsed.TotalSeconds + " s");
            Assert.True(result.Count == expectedCount);
        }

        [Test]
        [Ignore("Not testing this as Runv3 is not used in production code")]
        public void Test_PermutateV4_Runv3_PermutateOnSixSubject() {
            int expectedCount = 616872;
            var input = new List<Slot>();
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.Hydrology));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.StructuralAnalysisII));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.HighwayAndTransportation));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsII));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.IntroductionToBuildingServices));
            var timer_ofRunV3 = Stopwatch.StartNew();
            var result_ofRunV3 = Permutator.Run_v3(input.ToArray());
            timer_ofRunV3.Stop();
            Console.WriteLine("RunV3 result : ");
            Console.WriteLine("Combination count : " + result_ofRunV3.Count);
            Console.WriteLine("Elapsed time : " + timer_ofRunV3.Elapsed.TotalMilliseconds + " ms");

            var timer_ofRunV2 = Stopwatch.StartNew();
            var result_ofRunV2 = Permutator.Run_v2_WithConsideringWeekNumber(input.ToArray());
            timer_ofRunV2.Stop();
            Console.WriteLine("RunV2 result : ");
            Console.WriteLine("Combination count : " + result_ofRunV2.Count);
            Console.WriteLine("Elapsed time : " + timer_ofRunV2.Elapsed.TotalMilliseconds + " ms");


            Assert.Pass();



        }

        [Test]
        public void Test_PermutateV4_GenerateIndices() {
            var input = new List<List<Slot>>
            {
                new List<Slot> {new Slot()},
                new List<Slot> {new Slot(), new Slot()},
                new List<Slot> {new Slot(), new Slot(), new Slot()}
            };

            var actual = Permutator.GenerateIndices(input);
            var expected = new List<BoundedInt>
            {
                new BoundedInt(0, 0),
                new BoundedInt(1, 0),
                new BoundedInt(2, 0)
            };

            for (var i = 0 ; i < 3 ; i++) {
                if (!actual[i].Equals(expected[i])) Assert.Fail();
            }
            Assert.Pass();
        }




        [Test]
        public void Test_PermutateV4_Increment_1() {
            var input = new List<BoundedInt>
            {
                new BoundedInt(1, 0),
                new BoundedInt(2, 1),
                new BoundedInt(3, 2)
            };

            var expected = new List<BoundedInt>
            {
                new BoundedInt(1, 0),
                new BoundedInt(2, 1),
                new BoundedInt(3, 3)
            };
            var actual = Permutator.Increment(input);

            for (var i = 0 ; i < 3 ; i++) {
                if (!actual[i].Equals(expected[i])) Assert.Fail();
            }
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }

        [Test]
        public void Test_PermutateV4_Increment_2() {
            var input = new List<BoundedInt>
            {
                new BoundedInt(1, 0),
                new BoundedInt(2, 1),
                new BoundedInt(3, 3)
            };

            var expected = new List<BoundedInt>
            {
                new BoundedInt(1, 0),
                new BoundedInt(2, 2),
                new BoundedInt(3, 0)
            };
            var actual = Permutator.Increment(input);

            for (var i = 0 ; i < 3 ; i++) {
                if (!actual[i].Equals(expected[i])) Assert.Fail();
            }
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }

        [Test]
        public void Test_PermutateV4_Increment_3() {
            var input = new List<BoundedInt>
            {
                new BoundedInt(1, 0),
                new BoundedInt(2, 2),
                new BoundedInt(3, 3)
            };

            var expected = new List<BoundedInt>
            {
                new BoundedInt(1, 1),
                new BoundedInt(2, 0),
                new BoundedInt(3, 0)
            };
            var actual = Permutator.Increment(input);

            for (var i = 0 ; i < 3 ; i++) {
                if (!actual[i].Equals(expected[i])) Assert.Fail();
            }
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }

        [Test]
        public void Test_PermutateV4_Increment_4() {
            var input = new List<BoundedInt>
            {
                new BoundedInt(1, 0),
                new BoundedInt(0, 0),
                new BoundedInt(0, 0)
            };

            var expected = new List<BoundedInt>
            {
                new BoundedInt(1, 1),
                new BoundedInt(0, 0),
                new BoundedInt(0, 0)
            };
            var actual = Permutator.Increment(input);

            for (var i = 0 ; i < 3 ; i++) {
                if (!actual[i].Equals(expected[i])) Assert.Fail();
            }
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }

        [Test]
        public void Test_PermutateV4_Increment_5() {
            var input = new List<BoundedInt>
            {
                new BoundedInt(1, 1)
            };

            var actual = Permutator.Increment(input);
            Assert.True(actual == null);
        }

        [Test]
        public void Test_PermutateV4_Partitionize_1() {
            var input = new[]
            {
                new Slot {Code = "abc", Type = "L", Number = "1"},
                new Slot {Code = "abc", Type = "L", Number = "2"}
            };
            var actual = Permutator.Partitionize(input);
            var expected = new List<List<Slot>>
            {
                new List<Slot>(input.ToList())
            };
            if (actual.Count != expected.Count) Assert.Fail();
            foreach (var e in expected) {
                actual.RemoveAll(x => x.ScrambledEquals(e));
            }
            Assert.True(actual.Count == 0);
        }

        [Test]
        public void Test_PermutateV4_Partitionize_2() {
            var input = new[]
            {
                new Slot {Code = "abc", Type = "L", Number = "1"},
                new Slot {Code = "abc", Type = "T", Number = "1"}
            };
            var actual = Permutator.Partitionize(input);
            var expected = new List<List<Slot>>
            {
                new List<Slot> {input[0]},
                new List<Slot> {input[1]}
            };
            if (actual.Count != expected.Count) Assert.Fail();
            foreach (var e in expected) {
                actual.RemoveAll(x => x.ScrambledEquals(e));
            }
            Assert.True(actual.Count == 0);
        }

        [Test]
        public void Test_PermutateV4_Partitionize_3() {
            var input = new[]
            {
                new Slot {Code = "abc", Type = "L", Number = "1"},
                new Slot {Code = "abc", Type = "T", Number = "1"},
                new Slot {Code = "banana", Type = "L", Number = "1"}
            };
            var actual = Permutator.Partitionize(input);
            var expected = new List<List<Slot>>
            {
                new List<Slot> {input[0]},
                new List<Slot> {input[1]},
                new List<Slot> {input[2]}
            };
            if (actual.Count != expected.Count) Assert.Fail();
            foreach (var e in expected) {
                actual.RemoveAll(x => x.ScrambledEquals(e));
            }
            Assert.True(actual.Count == 0);
        }

        [Test]
        public void Test_PermutateV4_Partitionize_4() {
            var input = new[]
            {
                new Slot {Code = "abc", Type = "L", Number = "1"},
                new Slot {Code = "abc", Type = "T", Number = "1"},
                new Slot {Code = "banana", Type = "T", Number = "1"},
                new Slot {Code = "banana", Type = "T", Number = "2"}
            };
            var actual = Permutator.Partitionize(input);
            var expected = new List<List<Slot>>
            {
                new List<Slot> {input[0]},
                new List<Slot> {input[1]},
                new List<Slot> {input[2], input[3]}
            };
            if (actual.Count != expected.Count) Assert.Fail();
            foreach (var e in expected) {
                actual.RemoveAll(x => x.ScrambledEquals(e));
            }
            Assert.True(actual.Count == 0);
        }

        [Test]
        public void Test_PermutateV4_Partitionize_5() {
            var input = new[]
            {
                new Slot {Code = "abc", Type = "L", Number = "1"},
                new Slot {Code = "abc", Type = "T", Number = "1"},
                new Slot {Code = "banana", Type = "T", Number = "1"},
                new Slot {Code = "banana", Type = "T", Number = "2"},
                new Slot {Code = "abc", Type = "T", Number = "2"}
            };
            var actual = Permutator.Partitionize(input);
            var expected = new List<List<Slot>>
            {
                new List<Slot> {input[0]},
                new List<Slot> {input[1], input[4]},
                new List<Slot> {input[2], input[3]}
            };
            if (actual.Count != expected.Count) Assert.Fail();
            foreach (var e in expected) {
                actual.RemoveAll(x => x.ScrambledEquals(e));
            }
            Assert.True(actual.Count == 0);
        }

        [Test]
        public void Test_PermutateV4_Partitionize_6() {
            var input = new[]
            {
                new Slot {Code = "abc", Type = "L", Number = "1"},
                new Slot {Code = "abc", Type = "L", Number = "2"},
                new Slot {Code = "abc", Type = "L", Number = "3"},
                new Slot {Code = "abc", Type = "L", Number = "1"},
                new Slot {Code = "abc", Type = "L", Number = "2"},
                new Slot {Code = "abc", Type = "L", Number = "3"}
            };
            var actual = Permutator.Partitionize(input);
            var expected = new List<List<Slot>>
            {
                new List<Slot> {input[0], input[1], input[2]},
                new List<Slot> {input[3], input[4], input[5]}
            };
            if (actual.Count != expected.Count) Assert.Fail();
            foreach (var e in expected) {
                actual.RemoveAll(x => x.ScrambledEquals(e));
            }
            Assert.True(actual.Count == 0);
        }

        [Test]
        public void Test_PermutateV4_RemoveRelated() { }
    }
}