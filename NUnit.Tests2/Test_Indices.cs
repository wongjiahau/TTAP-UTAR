using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using static Time_Table_Arranging_Program.PermutateV4;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_Indices {
        [TestCase(new[] { 0 , 0 } , ExpectedResult = false)]
        [TestCase(new[] { 0 , 1 } , ExpectedResult = true)]
        public bool Test_Indices_Increment(int[] upperLimits) {
            var input = new Indices(upperLimits);
            return input.Increment();
        }

        [Test]
        public void Test_Indices_GetCurrentIndices() {
            var input = new Indices(0 , 1);
            input.Increment();
            bool result = input.GetCurrentIndices()[0].Value == 0
                          &&
                          input.GetCurrentIndices()[1].Value == 1;

            Assert.True(result);
        }

        [Test]
        public void Test_Indices_Count() {
            var input = new Indices(0 , 1);
            Assert.True(input.Count == 2);
        }

        [Test]
        public void Test_Indices_Indexer() {
            var input = new Indices(0 , 1);
            input.Increment();
            Assert.True(input[1].Value == 1);
        }

        [Test]
        public void Test_Indices_CurrentIndicesContainCrashedIndex_1() {
            var input = new Indices(0 , 1 , 2);
            input.AddNewCrashedIndex(new[] { new KeyValuePair(1 , 0) , new KeyValuePair(2 , 0) });
            Assert.True(input.CurrentIndicesContainCrashedIndex());
        }

        [Test]
        public void Test_Indices_CurrentIndicesContainCrashedIndex_2() {
            var input = new Indices(0 , 1 , 2);
            input.AddNewCrashedIndex(new[] { new KeyValuePair(1 , 0) , new KeyValuePair(2 , 0) });
            input.Increment();
            Assert.False(input.CurrentIndicesContainCrashedIndex());
        }
    }
}
