using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Model;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_SubjectSchema {
        [Test]
        public void Test_Constructor_ThrowException() {
            var inputSlots = new List<Slot>();
            inputSlots.AddRange(TestData.GetSlotsByName(TestData.Subjects.AdvancedStructuralSteelDesign));
            inputSlots.AddRange(TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsI));
            Assert.Throws<ArgumentException>(() => {
                new SubjectSchema(inputSlots);
            });
        }

        [Test]
        public void Test_Constructor_1() {
            var s = new SubjectSchema(TestData.GetSlotsByName(TestData.Subjects.AdvancedStructuralSteelDesign));
            Assert.AreEqual("UEMX4313" , s.SubjectCode);
            Assert.IsTrue(s.GotLecture);
            Assert.IsTrue(s.GotTutorial);
            Assert.IsFalse(s.GotPractical);
        }

        [Test]
        public void Test_Constructor_2() {
            var s = new SubjectSchema(TestData.GetSlotsByName(TestData.Subjects.ArtCraftAndDesign));
            Assert.AreEqual("MPU34072" , s.SubjectCode);
            Assert.IsTrue(s.GotLecture);
            Assert.IsFalse(s.GotTutorial);
            Assert.IsFalse(s.GotPractical);
        }

        [Test]
        public void Test_Constructor_3() {
            var s = new SubjectSchema(TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsI));
            Assert.AreEqual("UEME2123" , s.SubjectCode);
            Assert.IsTrue(s.GotLecture);
            Assert.IsTrue(s.GotTutorial);
            Assert.IsTrue(s.GotPractical);
        }

        [Test]
        public void Test_Constructor_PassedInEmptyList() {
            var s = new SubjectSchema(new List<Slot>());
            Assert.IsFalse(s.GotLecture);
            Assert.IsFalse(s.GotTutorial);
            Assert.IsFalse(s.GotPractical);
        }

        [Test]
        public void Test_ConformsTo_1() {
            var targetSchema = new SubjectSchema(new List<Slot>()
                {TestData.GetSlot(167), TestData.GetSlot(171), TestData.GetSlot(207)});
            var schemaToBeValidated = new SubjectSchema(new List<Slot>()
                {TestData.GetSlot(167), TestData.GetSlot(171)});
            string result = schemaToBeValidated.ConformsTo(targetSchema);
            Console.WriteLine(result);
            Assert.IsTrue(result.Contains("At least one TUTORIAL is needed"));
        }

        [Test]
        public void Test_ConformsTo_2() {
            var targetSchema = new SubjectSchema(new List<Slot>()
                {TestData.GetSlot(167), TestData.GetSlot(171), TestData.GetSlot(207)});
            var schemaToBeValidated = new SubjectSchema(new List<Slot>()
                {TestData.GetSlot(167), TestData.GetSlot(171), TestData.GetSlot(207)});
            string result = schemaToBeValidated.ConformsTo(targetSchema);
            Assert.IsTrue(result == null);
        }
    }
}
