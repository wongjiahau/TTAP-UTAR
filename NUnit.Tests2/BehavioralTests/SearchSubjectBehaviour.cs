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
    public class SearchSubjectBehaviour {
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
            string behaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali had type something in the search box
            Then Ali shall see the HintLabel
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("something");
            Assert.IsTrue(input.IsHintLabelVisible, behaviour);
        }


        [Test]
        public void VisibilityOfHintLabel_2() {
            string behaviour =
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali had type something in the search box
            And Ali clear the searches after that
            Then Ali shall not see the HintLabel
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("something");
            input.Search("");
            Assert.IsFalse(input.IsHintLabelVisible, behaviour);
        }

        [Test]
        public void FilterSubject() {
            string behaviour=
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali search for 'ASSD'
            Then Ali shall see only one subject is visible to him
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("ASSD");
            Assert.IsTrue(input.NumberOfVisibleSubject() == 1, behaviour);
        }

        [Test]
        public void SuggestiveText_1() {
            string behaviour = 
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali search for 'bahsa'
            Then Ali shall see a suggestion 
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("bahsa");
            Assert.IsTrue(input.IsFeedbackPanelVisible, behaviour);
        }
 
        [Test]
        public void SuggestiveText_2() {
            string behaviour = 
                @"
            Given Ali just loaded slots data (by logging in)
            When Ali search for 'bahsa'
            Then Ali shall see a suggestion suggesting 'Bahasa'
                ";
            var input = new SubjectListModel(SubjectModel.Parse(TestData.TestSlots));
            input.Search("bahsa");
            Assert.IsTrue(input.SuggestedText == "Bahasa", behaviour);
        }
    }
}
