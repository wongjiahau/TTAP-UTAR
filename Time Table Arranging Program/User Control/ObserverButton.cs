using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.User_Control {
    public class ObserverButton : Button, IPureObserver<ITimetableList> {
        private IImmutableObservable<ITimetableList> _observableTimetable;

        public void SetObservedThings(IImmutableObservable<ITimetableList> x) {
            _observableTimetable = x;
            _observableTimetable.RegisterObserver(this);
        }

        public void Update() {
            ITimetableList currentState = _observableTimetable.GetCurrentState();
            if (currentState == TimetableList.NoPossibleCombination ||
                currentState == TimetableList.NoSlotsIsChosen) {
                IsEnabled = false;
            }
            else {
                IsEnabled = true;
            }
        }
    }
}