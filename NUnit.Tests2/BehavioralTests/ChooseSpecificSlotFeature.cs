using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.TokenParser;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.User_Control.SubjectListFolder;

namespace NUnit.Tests2.BehavioralTests {
    [TestFixture]
    public class ChooseSpecificSlotFeature {
        private ChooseSpecificSlotModel Input_1() {
            var subjectListModel = new SubjectListModel(SubjectModel.Parse(new HtmlSlotParser().Parse(Helper.RawStringOfTestFile("QingShengSampleHtmlData.html"))) , Permutator.Run_v2_withoutConsideringWeekNumber , new TaskRunnerForUnitTesting());
            var outputTimetables = new List<List<Slot>>();
            subjectListModel.NewListOfTimetablesGenerated += (sender , args) => {
                outputTimetables = (List<List<Slot>>)sender;
            };
            subjectListModel.SelectSubject("MPU3143"); //BMK2
            subjectListModel.SelectSubject("UKMM1043"); //BEAM
            return new ChooseSpecificSlotModel(subjectListModel.ToList().FindAll(x => x.IsSelected) , Permutator.Run_v2_withoutConsideringWeekNumber);
        }

        private ChooseSpecificSlotModel Input_2() {
            var subjectListModel = new SubjectListModel(SubjectModel.Parse(new HtmlSlotParser().Parse(Helper.RawStringOfTestFile("QingShengSampleHtmlData.html"))) , Permutator.Run_v2_withoutConsideringWeekNumber , new TaskRunnerForUnitTesting());
            var outputTimetables = new List<List<Slot>>();
            subjectListModel.NewListOfTimetablesGenerated += (sender , args) => {
                outputTimetables = (List<List<Slot>>)sender;
            };
            subjectListModel.SelectSubject("MPU3143"); //BMK2
            return new ChooseSpecificSlotModel(subjectListModel.ToList().FindAll(x => x.IsSelected) , Permutator.Run_v2_withoutConsideringWeekNumber);
        }

        [Test]
        public void ClickDoneDirectly() {
            string behavior = 
                @"
                    Given Ali chosen subject BMK2
                    And Ali opened the ChooseSpecificSlot window
                    When Ali clicked DONE button directly
                    Then Ali shall go back to the previous window without error
                ";
            var input = Input_2();
            input.CheckForError();
            Console.WriteLine(input.ErrorMessage);
            Assert.IsFalse(input.GotError, behavior);
            Assert.IsFalse(input.SlotSelectionIsChanged);
        }

        [Test]
        public void DeselectedAllSlots() {
            string behavior = 
                @"
                    Given Ali chosen subject BMK2
                    And Ali opened the ChooseSpecificSlot window
                    When Ali deselected all slots 
                    Then he should see error messages
                ";
            var input = Input_2();
            input.DeselectSlot(4);
            input.CheckForError();
            Console.WriteLine(input.ErrorMessage);
            Assert.IsTrue(input.GotError, behavior);
            Assert.IsTrue(input.SlotSelectionIsChanged);
        }
    }
}
