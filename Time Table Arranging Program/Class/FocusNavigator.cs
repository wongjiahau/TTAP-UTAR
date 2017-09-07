using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class {
    public interface IFocusable {
        bool IsFocused { get; set; }
    }

    public class MockFocusableObject : IFocusable {
        public bool IsFocused { get; set; }
    }

    public class FocusNavigator {
        private CyclicIteratableList<IFocusable> _iteratableList;
        private static IFocusable _lastFocused;
        public FocusNavigator(List<IFocusable> focusables) {
            _iteratableList = new CyclicIteratableList<IFocusable>(focusables);
        }

        public IFocusable GetCurrentlyFocusedItem() {
            return _lastFocused;
        }

        public void FocusFirstItem() {
            DefocusLastFocused();
            var current = _iteratableList.GetCurrent();
            if (current != null) current.IsFocused = true;
        }

        public void NavigateToNext() {
            _iteratableList.GoToNext();
            DefocusLastFocused();
            var current = _iteratableList.GetCurrent();
            if (current != null) current.IsFocused = true;
        }

        public void NavigateToPrevious() {
            _iteratableList.GoToPrevious();
            DefocusLastFocused();
            var current = _iteratableList.GetCurrent();
            if (current != null) current.IsFocused = true;
        }

        private void DefocusLastFocused() {
            if (_lastFocused != null) _lastFocused.IsFocused = false;
            _lastFocused = _iteratableList.GetCurrent();
        }
    }
}
