using NUnit.Framework;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.User_Control;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_Class_Subject {
        [Test]
        public void Test_Constructor() {
            var input = new List<Slot>();
            input.Add(TestData.GetSlot(231));
            input.Add(TestData.GetSlot(237));
            input.Add(TestData.GetSlot(263));
            var result = new Subject(input);
            var r = result;
            Assert.True(
                r.Lecture == "L1" &&
                r.Tutorial == "T5" &&
                r.Practical == "P6"
                );

        }
    }
}
