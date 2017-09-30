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
            var subjectListModel = new SubjectListModel(SubjectModel.Parse(TestData.Default()) , Permutator.Run_v2_withoutConsideringWeekNumber , new TaskRunnerForUnitTesting());
            var outputTimetables = new List<List<Slot>>();
            subjectListModel.NewListOfTimetablesGenerated += (sender , args) => {
                outputTimetables = (List<List<Slot>>)sender;
            };
            subjectListModel.SelectSubject("MPU3143"); //BMK2
            subjectListModel.SelectSubject("UKMM1043"); //BEAM
            return new ChooseSpecificSlotModel(subjectListModel.ToList().FindAll(x => x.IsSelected) , Permutator.Run_v2_withoutConsideringWeekNumber);
        }

        private ChooseSpecificSlotModel Input_2() {
            var subjectListModel = new SubjectListModel(SubjectModel.Parse(TestData.Default()) , Permutator.Run_v2_withoutConsideringWeekNumber , new TaskRunnerForUnitTesting());
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
            Assert.IsFalse(input.IsSlotSelectionChanged);
        }

        [Test]
        public void DeselectedSomeSlots() {
            string behavior =
                @"
                    Given Ali chosen subject BMK2
                    And Ali opened the ChooseSpecificSlot window
                    When Ali deselected Lecture 1 of BMK2
                    Then he should see error messages
                ";
            var input = Input_2();
            input.Subjects.Find(x => x.Code == "MPU3143").ToggleSlotSelection(4);
            input.CheckForError();
            Console.WriteLine(input.ErrorMessage);
            Assert.IsTrue(input.GotError , behavior);
            Assert.IsTrue(input.IsSlotSelectionChanged);
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
            input.Subjects.Find(x=>x.Code == "MPU3143").ToggleAllSlotSelectionCommand.Execute(null);
            input.CheckForError();
            Console.WriteLine(input.ErrorMessage);
            Assert.IsTrue(input.GotError, behavior);
            Assert.IsTrue(input.IsSlotSelectionChanged);
        }
 
        [Test]
        public void SelectedSlotsWhichGenerateNoPossibleTimetables() {
            string behavior = 
                @"
                    Given Ali chosen subject BMK2 & BEAM
                    And Ali opened the ChooseSpecificSlot window
                    When Ali deselected some slots on both subjects
                    And those slots would not generate any possible timetable
                    Then he should see error messages
                ";
            var input = Input_1();
            var subject_UKMM1043 = input.Subjects.Find(x=>x.Code == "UKMM1043");
            subject_UKMM1043.ToggleAllSlotSelectionCommand.Execute(null);
            subject_UKMM1043.ToggleSlotSelection(28);
            subject_UKMM1043.ToggleSlotSelection(33);
            input.CheckForError();
            Console.WriteLine(input.ErrorMessage);
            Assert.IsTrue(input.GotError, behavior);
            Assert.IsTrue(input.IsSlotSelectionChanged);
        }
    }
}
