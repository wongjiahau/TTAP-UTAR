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

namespace NUnit.Tests2.BehavioralTests {
    [TestFixture]
    public class SearchSubjectexpectedBehaviour {
        [Test]
        public void InitialState() {
            string behvaiour =
                @"
            Given Ali just loaded slots data (by logging in)
            Then Ali shall see only the SearchBox is visible 
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            Assert.IsFalse(input.IsErrorLabelVisible , behvaiour);
            Assert.IsFalse(input.IsFeedbackPanelVisible , behvaiour);
            Assert.IsFalse(input.IsHintLabelVisible , behvaiour);
        }

        [Test]
        public void VisibilityOfHintLabel_1() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali had type something in the search box
            Then Ali shall see the HintLabel
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("something");
            Assert.IsTrue(input.IsHintLabelVisible , expectedBehaviour);
        }


        [Test]
        public void VisibilityOfHintLabel_2() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali had type something in the search box
            And Ali clear the searches after that
            Then Ali shall not see the HintLabel
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("something");
            input.Search("");
            Assert.IsFalse(input.IsHintLabelVisible , expectedBehaviour);
        }

        [Test]
        public void FilterSubject() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali search for 'ASSD'
            Then Ali shall see only one subject is visible to him
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("ASSD");
            Assert.IsTrue(input.NumberOfVisibleSubject() == 1 , expectedBehaviour);
        }

        [Test]
        public void SuggestiveText_1() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali search for 'bahsa'
            Then Ali shall see a suggestion 
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("bahsa");
            Assert.IsTrue(input.IsFeedbackPanelVisible , expectedBehaviour);
        }

        [Test]
        public void SuggestiveText_2() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali search for 'bahsa'
            Then Ali shall see a suggestion suggesting 'Bahasa'
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("bahsa");
            Assert.IsTrue(input.SuggestedText == "Bahasa" , expectedBehaviour);
        }

        [Test]
        public void SuggestiveText_3() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali search for 'bahsa'
            And Ali clear the searches after that
            Then Ali shall not see any suggestion
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("bahsa");
            input.Search("");
            Assert.IsFalse(input.IsFeedbackPanelVisible , expectedBehaviour);
        }

        [Test]
        public void ErrorLabel_1() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali search for 'some random text lol lol lol'
            Then Ali shall not see any suggestion
            And Ali shall see an error message
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("some random text lol lol lol");
            Assert.IsTrue(input.IsErrorLabelVisible , expectedBehaviour);
            Assert.IsFalse(input.IsFeedbackPanelVisible , expectedBehaviour);
        }

        [Test]
        public void ErrorLabel_2() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali search for 'some random text lol lol lol'
            And Ali clear searches after that
            Then Ali shall not see any suggestion
            And Ali shall not see the error message anymore 
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("some random text lol lol lol");
            input.Search("");
            Assert.IsFalse(input.IsErrorLabelVisible , expectedBehaviour);
        }

        [Test]
        public void DisplayMode_1() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected 1 subject
            And Ali clicked 'Show selected subject'
            Then Ali shall only see the subject he selected just now
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            models[0].IsSelected = true;
            var input = new SubjectListModel(models);
            input.ToggleDisplayModeCommand.Execute(null);
            Assert.IsTrue(input.NumberOfVisibleSubject() == 1 , expectedBehaviour);
        }

        [Test]
        public void DisplayMode_2() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected 1 subject
            And Ali clicked 'Show selected subject'
            And Ali clicked 'Show selected subject again'
            Then Ali shall see all the subjects 
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            models[0].IsSelected = true;
            var input = new SubjectListModel(models);
            input.ToggleDisplayModeCommand.Execute(null);
            input.ToggleDisplayModeCommand.Execute(null);
            Assert.IsTrue(input.NumberOfVisibleSubject() == models.Count , expectedBehaviour);
        }

        [Test]
        public void DisplayMode_3() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali selected 1 subject
            And Ali clicked 'Show selected subject'
            And Ali start to type something in the search box
            Then Ali shall see all the subjects 
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            models[0].IsSelected = true;
            var input = new SubjectListModel(models);
            input.ToggleDisplayModeCommand.Execute(null);
            input.Search("");
            Assert.IsTrue(input.NumberOfVisibleSubject() == models.Count , expectedBehaviour);
        }

        [Test]
        public void NavigateUsingArrowKeys_InitialState() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            Then Ali should see that the first subject in the list is highlighted
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            var input = new SubjectListModel(models);
            Assert.IsTrue(models[0].IsFocused , expectedBehaviour);
        }

        [Test]
        public void NavigateUsingArrowKeys_1() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali focused the SelectSubjectPanel
            And Ali press Down arrow key
            Then Ali should see that the focus will move to the 2nd subject in the list
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            var input = new SubjectListModel(models);
            input.NavigateToNextSubjectCommand.Execute(null);
            Assert.IsTrue(models[1].IsFocused , expectedBehaviour);
        }

        [Test]
        public void NavigateUsingArrowKeys_2() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali focused the SelectSubjectPanel
            And Ali press Down arrow key
            And Ali press Up arrow key
            Then Ali should see that the first subject in the list is highlighted 
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            var input = new SubjectListModel(models);
            input.NavigateToNextSubjectCommand.Execute(null);
            input.NavigateToPreviousSubjectCommand.Execute(null);
            Assert.IsTrue(models[0].IsFocused , expectedBehaviour);
        }

        [Test]
        public void Focusing_TestingForBug() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali search for random text that results in no matching subject
            And Ali press arrow keys (Up or Down)
            Then Ali shall not crash the application
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            var input = new SubjectListModel(models);
            input.Search("qlkehj j2f2gkjg4");
            input.NavigateToNextSubjectCommand.Execute(null);
            input.NavigateToPreviousSubjectCommand.Execute(null);
            Assert.Pass(expectedBehaviour);
        }

        [Test]
        public void NavigatingAfterSearching_1() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali searched for 'Bahasa'
            Then Ali shall see that the FIRST matching subject in the list is highlighted
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            var input = new SubjectListModel(models);
            input.Search("Bahasa");
            Assert.IsTrue(models.FindAll(x => x.Name.ToLower().Contains("Bahasa".ToLower()))[0].IsFocused , expectedBehaviour);
        }

        [Test]
        public void NavigatingAfterSearching_2() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali searched for 'Bahasa'
            And Ali press Down Key
            Then Ali shall see that the SECOND matching subject in the list is highlighted
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            var input = new SubjectListModel(models);
            input.Search("Bahasa");
            input.NavigateToNextSubjectCommand.Execute(null);
            Assert.IsTrue(models.FindAll(x => x.Name.ToLower().Contains("Bahasa".ToLower()))[1].IsFocused , expectedBehaviour);
        }


        [Test]
        public void NavigatingAfterSearching_3() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali searched for 'Bahasa'
            And Ali press Down Key
            And Ali clear the searches
            Then Ali shall see that only ONE subject in the list is highlighted
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            var input = new SubjectListModel(models);
            input.Search("Bahasa");
            input.NavigateToNextSubjectCommand.Execute(null);
            input.Search("");
            Assert.IsTrue(models.Count(x => x.IsFocused) == 1 , expectedBehaviour);
        }

        [Test]
        public void NavigatingAfterSearching_4() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali searched for the name of first subject in the list('ASSD')
            Then Ali shall see that the subject ('ASSD') is highlighted
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            var input = new SubjectListModel(models);
            input.Search("ASSD".ToLower());
            Assert.IsTrue(models.Find(x => x.CodeAndNameInitials.Contains("ASSD")).IsFocused
                , expectedBehaviour);
        }

        [Test]
        public void SelectSubjectByPressingEnter() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali searched for the name of first subject in the list('ASSD')
            And Ali pressed Enter  
            Then Ali shall see that the first subject in the list is selected
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            var input = new SubjectListModel(models , Permutator.Run_v2_withoutConsideringWeekNumber , new TaskRunnerForUnitTesting());
            input.Search("ASSD".ToLower());
            input.ToggleSelectionOnCurrentFocusedSubject();
            Assert.IsTrue(models.Find(x => x.CodeAndNameInitials.Contains("ASSD")).IsSelected
                , expectedBehaviour);
        }

        [Test]
        public void DeselectSubjectByPressingEnter() {
            string expectedBehaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali searched for the name of first subject in the list('ASSD')
            And Ali pressed Enter  
            Then Ali shall see that the first subject in the list is selected
                ";
            var models = SubjectModel.Parse(TestData.TestSlots);
            var input = new SubjectListModel(models , Permutator.Run_v2_withoutConsideringWeekNumber , new TaskRunnerForUnitTesting());
            input.Search("ASSD".ToLower());
            input.ToggleSelectionOnCurrentFocusedSubject();
            input.ToggleSelectionOnCurrentFocusedSubject();
            Assert.IsTrue(models.Find(x => x.CodeAndNameInitials.Contains("ASSD")).IsSelected == false
                , expectedBehaviour);
        }
    }
}
