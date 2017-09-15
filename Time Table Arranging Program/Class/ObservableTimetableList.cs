using System.Collections.Generic;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class {
    public class ObservableTimetableList : MutableObservable<ITimetableList>, ConvertibleToListOf<ITimetable> {
        public ObservableTimetableList(ITimetableList initialState) : base(initialState) { }

        public List<ITimetable> ToList() {
            //   if(_currentState == null) return new List<ITimetable>();
            return _currentState.ToList();
        }

        public ITimetableList GetLikedTimetableOnly() {
            var result = new List<ITimetable>();
            var timetableList = ToList();

            for (int i = 0; i < timetableList.Count; i++) {
                var timetable = timetableList[i];
                if (timetable.IsLiked) result.Add(timetable);
            }
            if (result.Count == 0) return TimetableList.NoLikedTimetable;
            return new TimetableList(result);
        }
    }
}