using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.AbstractClass;

namespace Time_Table_Arranging_Program.Model {
    public class ChooseSpecificSlotModel : ObservableObject {
        private readonly List<List<Slot>> _filteredTimetables = new List<List<Slot>>();
        public List<List<Slot>> NewListOfTimetables { get; private set; } = new List<List<Slot>>();
        public List<SubjectModel> SelectedSubjects { get; }

        [Obsolete("This constructor is for initialization of XAML designer only")]
        public ChooseSpecificSlotModel() { }

        public ChooseSpecificSlotModel(List<SubjectModel> selectedSubjects , List<List<Slot>> originalTimetables) {
            SelectedSubjects = selectedSubjects;
            NewListOfTimetables = originalTimetables;
            UpdateViewProperties();
        }

        public void SelectSlot(int uid) {
            for (int i = 0 ; i < _filteredTimetables.Count ; i++) {
                var timetable = _filteredTimetables[i];
                if (timetable.Any(slot => slot.UID == uid)) {
                    NewListOfTimetables.Add(timetable);
                    _filteredTimetables.Remove(timetable);
                    i--;
                }
            }
            UpdateViewProperties();
        }

        public void DeselectSlot(int uid) {
            for (int i = 0 ; i < NewListOfTimetables.Count ; i++) {
                var timetable = NewListOfTimetables[i];
                if (timetable.Any(slot => slot.UID == uid)) {
                    _filteredTimetables.Add(timetable);
                    NewListOfTimetables.Remove(timetable);
                    i--;
                }
            }
            UpdateViewProperties();
        }

        public void SelectSlots(List<int> uids) {
            for (int i = 0; i < uids.Count; i++) {
                SelectSlot(uids[i]);
            }
        }

        public void DeselectSlots(List<int> uids) {
            for (int i = 0; i < uids.Count; i++) {
                DeselectSlot(uids[i]);
            }
        }

        private void UpdateViewProperties() {
            if (NewListOfTimetables == null) return;
            NumberOfRemainingTimetables = NewListOfTimetables.Count;
            NumberOfRemovedTimetables = _filteredTimetables.Count;
        }
        #region ViewProperties
        private int _numberOfRemainingTimetables;
        public int NumberOfRemainingTimetables {
            get => _numberOfRemainingTimetables;
            set => SetProperty(ref _numberOfRemainingTimetables , value);
        }

        private int _numberOfRemovedTimetables;
        public int NumberOfRemovedTimetables {
            get => _numberOfRemovedTimetables;
            set => SetProperty(ref _numberOfRemovedTimetables , value);
        }


        #endregion

    }
}
