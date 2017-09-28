using System.Linq;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.TokenParser;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.User_Control.SubjectListFolder;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_SubjectModel {
        private SubjectListModel Input() {
            return new SubjectListModel(SubjectModel.Parse(TestData.TestSlots) ,
                Permutator.Run_v2_withoutConsideringWeekNumber , new TaskRunnerForUnitTesting());
        }

        private SubjectListModel Input2() {
            return new SubjectListModel(
                SubjectModel.Parse(new HtmlSlotParser().Parse(
                    Helper.RawStringOfTestFile("SampleData-FAM-2017-2ndSem.html"))) ,
                Permutator.Run_v2_withoutConsideringWeekNumber ,
                new TaskRunnerForUnitTesting());
        }

        private SubjectModel Input3() {
            return new SubjectListModel(
                SubjectModel.Parse(new HtmlSlotParser().Parse(
                    Helper.RawStringOfTestFile("SampleData-FAM-2017-2ndSem.html"))) ,
                Permutator.Run_v2_withoutConsideringWeekNumber ,
                new TaskRunnerForUnitTesting()).ToList()[0];
        }

        [Test]
        public void Test_SubjectModel_SlotsShouldBeDeselectedInitially() {
            var input = Input2();
            var subject_MPU34022 = input.ToList().Find(x => x.Code == "MPU34022");
            foreach (var s in subject_MPU34022.Slots) {
                Assert.IsFalse(s.IsSelected);
            }
        }

        [Test]
        public void Test_SubjectModel_IsSelected_1() {
            var input = Input2();
            input.SelectSubject("MPU34022");
            var subject_MPU34022 = input.ToList().Find(x => x.Code == "MPU34022");
            foreach (var s in subject_MPU34022.Slots) {
                Assert.IsTrue(s.IsSelected);
            }
        }

        [Test]
        public void Test_SubjectModel_IsSelected_2() {
            var input = Input();
            input.SelectSubject("MPU34072");
            var mpu34072 = input.ToList().Find(x => x.Code == "MPU34072");
            foreach (var s in mpu34072.Slots) {
                Assert.IsTrue(s.IsSelected);
            }
        }

        [Test]
        public void Test_SubjectModel_IsDeselected_1() {
            var input = Input();
            input.SelectSubject("MPU34072");
            input.SelectSubject("MPU34072" , false);
            var mpu34072 = input.ToList().Find(x => x.Code == "MPU34072");
            foreach (var s in mpu34072.Slots) {
                Assert.IsFalse(s.IsSelected);
            }

        }

        [Test]
        public void Test_StateOfSelectionOfAllSlots_1() {
            var input = Input3();
            Assert.IsFalse(input.Slots.All(x => x.IsSelected));
        }

        [Test]
        public void Test_StateOfSelectionOfAllSlots_2() {
            var input = Input3();
            Assert.IsFalse(input.IsAllSlotsSelected);
        }

        [Test]
        public void Test_StateOfSelectionOfAllSlots_3() {
            var input = Input3();
            input.IsSelected = true;
            Assert.IsTrue(input.Slots.All(x => x.IsSelected));
        }

        [Test]
        public void Test_StateOfSelectionOfAllSlots_4() {
            var input = Input3();
            input.IsSelected = true;
            Assert.IsTrue(input.IsAllSlotsSelected);
        }

        [Test]
        public void Test_ToggleAllSlotSelectionCommand() {
            var input = Input3();
            input.IsSelected = true;
            input.ToggleAllSlotSelectionCommand.Execute(null);
            Assert.IsFalse(input.IsAllSlotsSelected);
        }
    }
}
