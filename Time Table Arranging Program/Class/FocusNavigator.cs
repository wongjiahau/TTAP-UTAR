using System.Collections.Generic;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class {
    public interface IFocusable {
        bool IsFocused { get; set; }
        void SetSupervisor(ISupervisor focusNavigator);
    }

    public class MockFocusableObject : IFocusable {
        public bool IsFocused { get; set; }

        public void SetSupervisor(ISupervisor focusNavigator) {
            //Do nothing
        }
    }

    public interface ISupervisor {
        void FocusMe(IFocusable supervisee);
    }

    public class FocusNavigator : ISupervisor {
        private static IFocusable _lastFocused;
        private readonly CyclicIteratableList<IFocusable> _iteratableList;

        public FocusNavigator(List<IFocusable> focusables) {
            _iteratableList = new CyclicIteratableList<IFocusable>(focusables);
            foreach (var f in focusables) {
                f.SetSupervisor(this);
            }
        }

        public void FocusMe(IFocusable supervisee) {
            _lastFocused.IsFocused = false;
            supervisee.IsFocused = true;
            _lastFocused = supervisee;
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