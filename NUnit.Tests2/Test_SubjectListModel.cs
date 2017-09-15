using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.User_Control.SubjectListFolder;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_SubjectListModel {
        private SubjectListModel input() {
            return new SubjectListModel(SubjectModel.Parse(TestData.TestSlots), Permutator.Run_v2_withoutConsideringWeekNumber, new TaskRunnerForUnitTesting());
        }


        [Test]
        public void Test_SelectSubjectUsingRandomCodeShallThrowException() {
            var input = this.input();
            Assert.That(() => {
                input.SelectSubject("Random code");
            } ,
            Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Test_SubjectListModel_SelectSubjectUsingCorrectCode() {
            var input = this.input();
            input.SelectSubject("MPU3113");
            Assert.IsTrue(input.SelectedSubjectCount == 1);
        }
    }
}
