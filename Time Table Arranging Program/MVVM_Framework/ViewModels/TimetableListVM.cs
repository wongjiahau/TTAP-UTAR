using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.MVVM_Framework.Models;

namespace Time_Table_Arranging_Program.MVVM_Framework.ViewModels {
    public class TimetableListVM : ViewModelBase<TimetableList_2> {
        private readonly CyclicIndexVM _cyclicIndexVM;
        private readonly TimetableList_2 _timetableList;

        public TimetableListVM(TimetableList_2 timetableList, CyclicIndexVM cyclicIndexVm) {
            _timetableList = timetableList;
            _cyclicIndexVM = cyclicIndexVm;
        }

        public TimetableListVM() { }

        public ITimetable CurrentTimetable {
            get {
                if (_timetableList == null) return null;
                return _timetableList.Timetables[_cyclicIndexVM.CurrentValue - 1];
            }
        }

        public CyclicIndexVM CyclicIndexVM => _cyclicIndexVM;
    }
}