using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.User_Control.CheckboxWithListDownMenuFolder.ErrorMessageType;
using Time_Table_Arranging_Program.User_Control.SubjectListFolder;

namespace NUnit.Tests2.BehavioralTests {
    [TestFixture]
    public class SelectSubjectBehaviour {
        private SubjectListModel Input() {
            return new SubjectListModel(SubjectModel.Parse(TestData.TestSlots), Permutator.Run_v2_withoutConsideringWeekNumber);
        }

        [Test]
        public void ClashReporting_1() {
            string behvaiour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected subject 'MPU34072' (Art, Craft, And Design)
            And Then Ali selected subject 'UEMX4313' (Advanced Structural Steel Design)
            Then Ali shall see a clash report saying 'UEMX4313' cannot be selected
            And the subject UEMX4313 shall not be selected
                ";
            var input = this.Input();
            input.SelectSubject("MPU34072");
            input.SelectSubject("UEMX4313");
            var subject_UEMX4313 = input.ToList().Find(x => x.Code == "UEMX4313");
            Assert.IsTrue(subject_UEMX4313.ClashingErrorType == ClashingErrorType.SingleClashingError);
            Assert.IsFalse(subject_UEMX4313.IsSelected);
        }

        [Test]
        public void ClashReporting_2() {
            string behvaiour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected subject 'MPU34072' (Art, Craft, And Design)
            And Then Ali selected subject 'UEMX4313' (Advanced Structural Steel Design)
            And Then Ali deselected subject 'MPU34072' (Art, Craft, And Design)
            Then Ali shall see the clash report on UEMX4313 is dissappeared
            And the subject UEMX4313 shall not be selected
                ";
            var input = this.Input();
            input.SelectSubject("MPU34072");
            input.SelectSubject("UEMX4313");
            input.SelectSubject("MPU34072", false);
            var subject_UEMX4313 = input.ToList().Find(x => x.Code == "UEMX4313");
            Assert.IsTrue(subject_UEMX4313.ClashingErrorType == ClashingErrorType.NoError);
            Assert.IsFalse(subject_UEMX4313.IsSelected);
        }
    }
}
