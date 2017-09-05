using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
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
            Assert.IsFalse(input.IsErrorLabelVisible, behvaiour);
            Assert.IsFalse(input.IsFeedbackPanelVisible, behvaiour);
            Assert.IsFalse(input.IsHintLabelVisible, behvaiour);
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
            Assert.IsTrue(input.IsHintLabelVisible, expectedBehaviour);
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
            Assert.IsFalse(input.IsHintLabelVisible, expectedBehaviour);
        }

        [Test]
        public void FilterSubject() {
            string expectedBehaviour=
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali search for 'ASSD'
            Then Ali shall see only one subject is visible to him
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("ASSD");
            Assert.IsTrue(input.NumberOfVisibleSubject() == 1, expectedBehaviour);
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
            Assert.IsTrue(input.IsFeedbackPanelVisible, expectedBehaviour);
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
            Assert.IsTrue(input.SuggestedText == "Bahasa", expectedBehaviour);
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
            Assert.IsFalse(input.IsFeedbackPanelVisible, expectedBehaviour);
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
            Assert.IsTrue(input.IsErrorLabelVisible, expectedBehaviour);
            Assert.IsFalse(input.IsFeedbackPanelVisible, expectedBehaviour);
        }

        [Test]
        public void ErrorLabel_2() {
            string expectedBehaviour = 
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali search for 'some random text lol lol lol'
            And Ali clear searches after that
            Then Ali shall not see any suggestion
            And Ali shall see an error message
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("some random text lol lol lol");
            input.Search("");
            Assert.IsFalse(input.IsErrorLabelVisible, expectedBehaviour);
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
            Assert.IsTrue(input.NumberOfVisibleSubject() == 1, expectedBehaviour);
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
            Assert.IsTrue(input.NumberOfVisibleSubject() == models.Count, expectedBehaviour);
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
            Assert.IsTrue(input.NumberOfVisibleSubject() == models.Count, expectedBehaviour);
        }
 
    }
}
