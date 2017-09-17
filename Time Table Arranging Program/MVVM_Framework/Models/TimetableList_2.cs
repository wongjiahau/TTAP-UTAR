using System.Collections.Generic;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.AbstractClass;

namespace Time_Table_Arranging_Program.MVVM_Framework.Models {
    public class TimetableList_2 : ObservableObject {
        private List<ITimetable> _timetables;

        public TimetableList_2(List<ITimetable> timetables) {
            _timetables = timetables;
        }

        public List<ITimetable> Timetables {
            get { return _timetables; }
            set { SetProperty(ref _timetables, value); }
        }
    }
}