using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Model;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_SubjectSchema {

        [Test]
        public void Test_Constructor_1() {
            var s = new SubjectSchema(TestData.GetSlotsByName(TestData.Subjects.AdvancedStructuralSteelDesign));
            Assert.AreEqual("UEMX4313", s.SubjectCode);
            Assert.IsTrue(s.GotLecture);
            Assert.IsTrue(s.GotTutorial);
            Assert.IsFalse(s.GotPractical);
        }

        [Test]
        public void Test_Constructor_2() {
            var s = new SubjectSchema(TestData.GetSlotsByName(TestData.Subjects.ArtCraftAndDesign));
            Assert.AreEqual("MPU34072", s.SubjectCode);
            Assert.IsTrue(s.GotLecture);
            Assert.IsFalse(s.GotTutorial);
            Assert.IsFalse(s.GotPractical);
        }

        [Test]
        public void Test_Constructor_3() {
            var s = new SubjectSchema(TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsI));
            Assert.AreEqual("UEME2123", s.SubjectCode);
            Assert.IsTrue(s.GotLecture);
            Assert.IsTrue(s.GotTutorial);
            Assert.IsTrue(s.GotPractical);
        }

    }
}
