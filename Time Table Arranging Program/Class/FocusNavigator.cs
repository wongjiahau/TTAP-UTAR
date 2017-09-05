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
        private IFocusable _lastFocused;
        public FocusNavigator(List<IFocusable> focusables) {
            _iteratableList = new CyclicIteratableList<IFocusable>(focusables);
        }

        public void FocusFirstItem() {
            _iteratableList.GetCurrent().IsFocused = true;
            DefocusLastFocused();
        }

        public void NavigateToNext() {
            _iteratableList.GoToNext();
            _iteratableList.GetCurrent().IsFocused = true;
            DefocusLastFocused();
        }

        public void NavigateToPrevious() {
            _iteratableList.GoToPrevious();
            _iteratableList.GetCurrent().IsFocused = true;
            DefocusLastFocused();
        }

        private void DefocusLastFocused() {
            if (_lastFocused != null) _lastFocused.IsFocused = false;
            _lastFocused = _iteratableList.GetCurrent();
        }
    }
}
