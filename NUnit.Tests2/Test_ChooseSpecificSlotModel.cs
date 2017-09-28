using System.Collections.Generic;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.TokenParser;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.User_Control.SubjectListFolder;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_ChooseSpecificSlotModel {
        private ChooseSpecificSlotModel Input() {
            var subjectListModel = new SubjectListModel(SubjectModel.Parse(new HtmlSlotParser().Parse(Helper.RawStringOfTestFile("QingShengSampleHtmlData.html"))) , Permutator.Run_v2_withoutConsideringWeekNumber , new TaskRunnerForUnitTesting());
            var outputTimetables = new List<List<Slot>>();
            subjectListModel.NewListOfTimetablesGenerated += (sender , args) => {
                outputTimetables = (List<List<Slot>>)sender;
            };
            subjectListModel.SelectSubject("MPU3143"); //BMK2
            subjectListModel.SelectSubject("UKMM1043"); //BEAM
            return new ChooseSpecificSlotModel(subjectListModel.ToList().FindAll(x => x.IsSelected) , Permutator.Run_v2_withoutConsideringWeekNumber);
        }

        [Test]
        public void Test_InitialStateOf_SelectedSubjects() {
            var model = Input();
            Assert.IsTrue(model.Subjects.Count == 2);
        }

        [Test]
        public void Test_InitialStateOf_NewListOfTimetables() {
            var model = Input();
            Assert.IsTrue(model.NewListOfTimetables.Count == 0);
        }

        [Test]
        public void Test_InitialStateOf_GotError() {
            var model = Input();
            Assert.IsFalse(model.GotError);
        }

        [Test]
        public void Test_InitialStateOf_AllSlot_ShouldBeAllSelected() {
            var model = Input();
            foreach (Slot s in model.AllSlot) {
                if(s.IsSelected == false) 
                    Assert.Fail("All slots of AllSlot should be selected initially.");
            }
            Assert.Pass();
        }
   }
}
