using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Model {
    public interface IOutputTimetableModel : ICyclicIteratable<ITimetable>, IMutableObservable<ITimetableList> {
        void GenerateTimetables(SlotList slotList);
    }

    public class OutputTimetableModel : IOutputTimetableModel {
        private readonly MutableObservable<ITimetableList> _observable;
        private ITimetableList _timetableList;

        public OutputTimetableModel(ITimetableList timetableList) {
            _observable = new ObservableTimetableList(timetableList);
            _timetableList = timetableList;
        }

        public void GoToNext() {
            _timetableList.GoToNext();
            NotifyObserver();
        }

        public void GoToPrevious() {
            _timetableList.GoToPrevious();
            NotifyObserver();
        }

        public void GoToRandom() {
            _timetableList.GoToRandom();
            NotifyObserver();
        }

        public void GoTo(int index) {
            _timetableList.GoTo(index);
            NotifyObserver();
        }

        public int MaxIndex() {
            return _timetableList.MaxIndex();
        }

        public int CurrentIndex() {
            return _timetableList.CurrentIndex();
        }

        public ITimetable GetCurrent() {
            return _timetableList.GetCurrent();
        }


        public void GenerateTimetables(SlotList slotList) {
            var result = Permutator.Run_v2_WithConsideringWeekNumber(slotList.ToArray());
            _timetableList = new TimetableList(result);
            NotifyObserver();
        }

        public void RegisterObserver(IObserver o) {
            _observable.RegisterObserver(o);
        }

        public void RemoveObserver(IObserver o) {
            _observable.RemoveObserver(o);
        }

        public void NotifyObserver() {
            _observable.NotifyObserver();
        }

        public ITimetableList GetCurrentState() {
            return _timetableList;
        }

        public void SetState(ITimetableList newState) {
            _timetableList = newState;
            NotifyObserver();
        }

        public int Counts { get; }
    }
}