using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class {
    public class ObservableTimetable : MutableObservable<ITimetable> {
        public ObservableTimetable(ITimetable initialState) : base(initialState) { }
    }
}