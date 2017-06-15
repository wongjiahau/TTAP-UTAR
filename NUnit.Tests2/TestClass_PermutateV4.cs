using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;

namespace NUnit.Tests2 {
    [TestFixture]
    public class TestClass_PermutateV4 {
        private static readonly List<Slot> TestData = new List<Slot>
        {
            new Slot {SubjectName = "English", Type = "L", Number = ""}
        };

        [Test]
        public void Test_PermutateV4_GenerateIndices() {
            var input = new List<List<Slot>>
            {
                new List<Slot> {new Slot()},
                new List<Slot> {new Slot(), new Slot()},
                new List<Slot> {new Slot(), new Slot(), new Slot()}
            };

            var actual = PermutateV4.GenerateIndices(input);
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
            var actual = PermutateV4.Increment(input);

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
            var actual = PermutateV4.Increment(input);

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
            var actual = PermutateV4.Increment(input);

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
            var actual = PermutateV4.Increment(input);

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

            var actual = PermutateV4.Increment(input);
            Assert.True(actual == null);
        }

        [Test]
        public void Test_PermutateV4_Partitionize_1() {
            var input = new[]
            {
                new Slot {Code = "abc", Type = "L", Number = "1"},
                new Slot {Code = "abc", Type = "L", Number = "2"}
            };
            var actual = PermutateV4.Partitionize(input);
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
            var actual = PermutateV4.Partitionize(input);
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
            var actual = PermutateV4.Partitionize(input);
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
            var actual = PermutateV4.Partitionize(input);
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
            var actual = PermutateV4.Partitionize(input);
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
            var actual = PermutateV4.Partitionize(input);
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