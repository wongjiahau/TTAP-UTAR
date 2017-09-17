using System.Collections.Generic;
using NUnit.Framework;
using Time_Table_Arranging_Program;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_Class_ExtensionMethods {
        [Test]
        public void Test_ScrambledEquals_1() {
            var a = new List<int> {1, 2, 3, 4, 4};
            var b = new List<int> {2, 3, 4, 4, 1};
            Assert.True(a.ScrambledEquals(b));
        }

        [Test]
        public void Test_ScrambledEquals_2() {
            var a = new List<int> {1, 2, 3, 4, 4};
            var b = new List<int> {2, 2, 4, 4, 1};
            Assert.False(a.ScrambledEquals(b));
        }

        [Test]
        public void Test_GetSelectedSlots() {
            var input = TestData.GetSubjectListModel();
            input.SelectSubject("MPU34072");
            var expected = 1;
            var actual = input.ToList().GetSelectedSlots().Count;
            Assert.AreEqual(expected, actual);
        }
    }
}