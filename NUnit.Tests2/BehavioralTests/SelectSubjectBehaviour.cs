using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.User_Control.CheckboxWithListDownMenuFolder.ErrorMessageType;
using Time_Table_Arranging_Program.User_Control.SubjectListFolder;

namespace NUnit.Tests2.BehavioralTests {
    [TestFixture]
    public class SelectSubjectBehaviour {
        private SubjectListModel Input() {
            return new SubjectListModel(SubjectModel.Parse(TestData.TestSlots) , Permutator.Run_v2_withoutConsideringWeekNumber, new TaskRunnerForUnitTesting());
        }

        [Test]
        public void SubjectSelectionShouldTriggerUpdateToUI() {
            string behaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected subject 'MPU34072' (Art, Craft, And Design)
            Then Ali shall notice the TimetableGUI is updated
                ";
            var subjectListModel = Input();
            subjectListModel.NewListOfTimetablesGenerated += (sender , args) => {
                Assert.Pass();
            };
            subjectListModel.SelectSubject("MPU34072");
            Assert.Fail(behaviour);
        }

        [Test]
        public void SubjectDeselectionShouldTriggerUpdateToUi() {
            string behaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected a subject X 
            And Then Ali selected another subject Y 
            And Then Ali deselected subject X
            Then Ali shall notice the TimetableGUI is updated
                ";
            var subjectListModel = Input();
            subjectListModel.SelectSubject("MPU34072");
            subjectListModel.SelectSubject("UEMX4313");
            subjectListModel.NewListOfTimetablesGenerated += (sender , args) => {
                Assert.Pass();
            };
            subjectListModel.SelectSubject("MPU34072" , false);
            Assert.Fail(behaviour);
        }

        [Test]
        public void SelectingClashingSubjectShouldNotUpdateGui() {
            string behaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected a subject X 
            And Then Ali selected subject Y which is clashing with X
            Then Ali shall not see that the TimetableGui is being updated
                ";
            var input = Input();
            input.SelectSubject("MPU34072");
            input.NewListOfTimetablesGenerated += (sender , args) => {
                Assert.Fail(behaviour);
            };
            input.SelectSubject("UEMX4313");
            Assert.Pass();
        }

        [Test]
        public void SelectingClashingSubjectShouldNotUpdateSelectedSubjectCount() {
            string behaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected a subject X 
            And Then Ali selected subject Y which is clashing with X
            Then Ali shall see that the SelectedSubjectCount is still unchanged
                ";
            var input = Input();
            input.SelectSubject("MPU34072");
            input.SelectSubject("UEMX4313");
            Assert.AreEqual(1 , input.SelectedSubjectCount);
        }

        [Test]
        public void ClashReporting_1() {
            string behaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected subject 'MPU34072' (Art, Craft, And Design)
            And Then Ali selected subject 'UEMX4313' (Advanced Structural Steel Design)
            Then Ali shall see a clash report saying 'UEMX4313' cannot be selected
            And the subject UEMX4313 shall not be selected
                ";
            var input = Input();
            input.SelectSubject("MPU34072");
            input.SelectSubject("UEMX4313");
            var subject_UEMX4313 = input.ToList().Find(x => x.Code == "UEMX4313");
            Assert.IsTrue(subject_UEMX4313.ClashingErrorType == ClashingErrorType.SingleClashingError , behaviour);
            Assert.IsFalse(subject_UEMX4313.IsSelected , behaviour);
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
            var input = Input();
            input.SelectSubject("MPU34072");
            input.SelectSubject("UEMX4313");
            input.SelectSubject("MPU34072" , false);
            var subject_UEMX4313 = input.ToList().Find(x => x.Code == "UEMX4313");
            Assert.IsTrue(subject_UEMX4313.ClashingErrorType == ClashingErrorType.NoError , behvaiour);
            Assert.IsFalse(subject_UEMX4313.IsSelected , behvaiour);
        }

        [Test]
        public void ClashReporting_3() {
            string behaviour1 =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected a subject X  
            And Ali selected two subjects(Y, Z) which is clashing with X
            Then Ali shall see that clash report for Y and Z appear
                ";

            string behaviour2 =
                @"
            And Then When Ali deslected subject X
            Then Ali shall see that the clash report of Y, Z is dissappeared
                ";
            var input = Input();
            input.SelectSubject("UEMX4313"); //Subject X = Advanced structural steel design
            input.SelectSubject("MPU34072"); //Subject Y = Art, Craft & Design
            input.SelectSubject("UEMX2363"); //Subject Z = Concrete Structures Design II
            Assert.IsTrue(input.ToList().Find(x => x.Code == "MPU34072").ClashingErrorType == ClashingErrorType.SingleClashingError , behaviour1);
            Assert.IsTrue(input.ToList().Find(x => x.Code == "UEMX2363").ClashingErrorType == ClashingErrorType.SingleClashingError , behaviour1);

            input.SelectSubject("UEMX4313" , false); //Subject X = Advanced structural steel design
            Assert.IsTrue(input.ToList().Find(x => x.Code == "MPU34072").ClashingErrorType == ClashingErrorType.NoError , behaviour2);
            Assert.IsTrue(input.ToList().Find(x => x.Code == "UEMX2363").ClashingErrorType == ClashingErrorType.NoError , behaviour2);

        }

        [Test]
        public void ClashReporting_4() {
            string behvaiour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected subject X
            And Then Ali selected subject Y which is clashing with X
            And Then Ali selected subject Z
            And Then Ali selected subject A which is clashing with Z
            Then When Ali deselected subject X
            Ali shall see that only the clash report on Y is cleared, 
                but the clash report on A still remains
                ";
            var input = Input();
            input.SelectSubject("UEMX4313"); //Subject X = ASSD
            input.SelectSubject("MPU34072"); //Subject Y = ACAD
            input.SelectSubject("MPU3143"); //Subject Z = BMK2 
            input.SelectSubject("UEMX4913"); //Subject A = IDP
            input.SelectSubject("UEMX4313" , false); //Subject X = ASSD
            var subject_MPU34072 = input.ToList().Find(x => x.Code == "MPU34072");
            var subject_UEMX4913 = input.ToList().Find(x => x.Code == "UEMX4913");
            Assert.IsTrue(subject_MPU34072.ClashingErrorType == ClashingErrorType.NoError , behvaiour);
            Assert.IsTrue(subject_UEMX4913.ClashingErrorType == ClashingErrorType.SingleClashingError , behvaiour);
        }

        [Test]
        public void ClashReporting_GroupClashing() {
            string behaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected a bunch of subjects (A, B, C ... )
            And Then Ali selected a subject X which causes group clashing error
            Then Ali shall see that a clash report will appear for subject X
                ";
            var input = Input();
            input.SelectSubject("MPU32013"); //Subject A = BKA
            input.SelectSubject("MPU3143"); //Subject B = BMK2
            input.SelectSubject("MPU33183"); //Subject X = EIS
            Assert.IsTrue(input.ToList().Find(x => x.Code == "MPU33183").ClashingErrorType == ClashingErrorType.GroupClashingError, behaviour);
        }

        [Test]
        public void ClashReporting_GroupClashing_ReleasingGroupClashedSubject_1() {
            string behaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected a set of subjects (A, B, C) 
                which is the MINIMUM set that causes group clash if subject X is selected
            And Then Ali selected a subject X which causes group clashing error
            And Then Ali deselected one of the subject (one from A, B, C)
            Then Ali shall see that subject X is enabled again
                ";
            var input = Input();
            input.SelectSubject("MPU32013"); //Subject A = BKA
            input.SelectSubject("MPU3143"); //Subject B = BMK2
            input.SelectSubject("MPU33183"); //Subject X = EIS
            input.SelectSubject("MPU32013" , false); //Subject A = BKA
            Assert.IsTrue(input.ToList().Find(x => x.Code == "MPU33183").ClashingErrorType == ClashingErrorType.NoError, behaviour);

        }

        [Test]
        public void ClashReporting_GroupClashing_ReleasingGroupClashedSubject_2() {
            string behaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected a set of subjects (A, B, C) 
                which is a MINIMUM set that causes group clash if subject X is selected
            And Then Ali selected a subject X which causes group clashing error
            And Then Ali selected a subject D 
            And Then Ali deselected subject D
            Then Ali shall see that subject X is still disabled
                ";
            var input = Input();
            input.SelectSubject("MPU32013"); //Subject A = BKA
            input.SelectSubject("MPU3143"); //Subject B = BMK2
            input.SelectSubject("MPU33183"); //Subject X = EIS
            input.SelectSubject("UEMX4313"); //Subject X = ASSD
            input.SelectSubject("UEMX4313", false); //Subject X = ASSD
            Assert.IsTrue(input.ToList().Find(x => x.Code == "MPU33183").
                ClashingErrorType == ClashingErrorType.GroupClashingError, behaviour);
        }
    }
}
