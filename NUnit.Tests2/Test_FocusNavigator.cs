using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_FocusNavigator {
        private List<IFocusable> GetMockData() {
            var result = new List<IFocusable>();
            for (int i = 0 ; i < 10 ; i++) {
                result.Add(new MockFocusableObject());
            }
            return result;
        }

        [Test]
        public void Test_FocusNavigator_FocusFirstItem() {
            var input = GetMockData();
            var focusNavigator = new FocusNavigator(input);
            focusNavigator.FocusFirstItem();
            Assert.IsTrue(input[0].IsFocused);
        }

        [Test]
        public void Test_FocusNavigator_Navigate_1() {
            var input = GetMockData();
            var focusNavigator = new FocusNavigator(input);
            focusNavigator.NavigateToNext();
            Assert.IsTrue(input[1].IsFocused);
        }

        [Test]
        public void Test_FocusNavigator_Navigate_2() {
            var input = GetMockData();
            var focusNavigator = new FocusNavigator(input);
            focusNavigator.NavigateToNext();
            focusNavigator.NavigateToNext();
            Assert.IsTrue(input[2].IsFocused);
        }

        [Test]
        public void Test_FocusNavigator_Navigate_3() {
            var input = GetMockData();
            var focusNavigator = new FocusNavigator(input);
            focusNavigator.NavigateToNext();
            focusNavigator.NavigateToPrevious();
            Assert.IsTrue(input[0].IsFocused);
        }

        [Test]
        public void Test_FocusNavigator_Navigate_4() {
            var input = GetMockData();
            var focusNavigator = new FocusNavigator(input);
            focusNavigator.NavigateToPrevious();
            Assert.IsTrue(input[input.Count - 1].IsFocused);
        }

        [Test]
        public void Test_FocusNavigator_Navigate_5() {
            var input = GetMockData();
            var focusNavigator = new FocusNavigator(input);
            focusNavigator.NavigateToPrevious();
            focusNavigator.NavigateToNext();
            Assert.IsTrue(input[0].IsFocused);
        }

        [Test]
        public void Test_FocusNavigator_OnlyOneItemShouldBeFocusedAtATime() {
            var input = GetMockData();
            var focusNavigator = new FocusNavigator(input);
            focusNavigator.NavigateToNext();
            focusNavigator.NavigateToNext();
            Assert.IsTrue(input.Count(x => x.IsFocused) == 1);
        }
    }
}
