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
            return new SubjectListModel(SubjectModel.Parse(TestData.TestSlots) , Permutator.Run_v2_withoutConsideringWeekNumber);
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
            var input = this.Input();
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
            var input = this.Input();
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
            var input = this.Input();
            input.SelectSubject("UEMX4313"); //Subject X = Advanced structural steel design
            input.SelectSubject("MPU34072"); //Subject Y = Art, Craft & Design
            input.SelectSubject("UEMX2363"); //Subject Z = Concrete Structures Design II
            Assert.IsTrue(input.ToList().Find(x => x.Code == "MPU34072").ClashingErrorType == ClashingErrorType.SingleClashingError , behaviour1);
            Assert.IsTrue(input.ToList().Find(x => x.Code == "UEMX2363").ClashingErrorType == ClashingErrorType.SingleClashingError , behaviour1);

            input.SelectSubject("UEMX4313" , false); //Subject X = Advanced structural steel design
            Assert.IsTrue(input.ToList().Find(x => x.Code == "MPU34072").ClashingErrorType == ClashingErrorType.NoError, behaviour2);
            Assert.IsTrue(input.ToList().Find(x => x.Code == "UEMX2363").ClashingErrorType == ClashingErrorType.NoError, behaviour2);

        }

        [Test]
        public void ClashReporting_GroupClashing_1() {
            string behaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected a bunch of subjects 
            And Then Ali selected a subject X which causes group clashing error
            Then Ali shall see that a clash report will appear for subject X
                ";
            var input = this.Input();
            //TODO : Complete the code here
            Assert.Fail("Incomplete yet");

        }

        [Test]
        public void ClashReporting_GroupClashing_2() {
            string behaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected a bunch of subjects (A, B, C, D, E)
            And Then Ali selected a subject X which causes group clashing error
            And Then Ali deselected one of the subject (one from A, B, C, D, E)
            Then TTAP should recalculate to see if subject X still causes Group-Clashing error
            
                ";
            var input = this.Input();
            //TODO : Complete the code here
            Assert.Fail("Incomplete yet");

        }
    }
}
