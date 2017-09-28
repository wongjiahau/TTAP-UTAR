using System;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.AbstractClass;

namespace Time_Table_Arranging_Program.Model {
    public class ChooseSpecificSlotModel : ObservableObject {
        private Func<Slot[] , List<List<Slot>>> _permutator;
        private List<Slot> _allSlots = new List<Slot>();
        private List<SubjectSchema> _subjectSchemas = new List<SubjectSchema>();
        public List<List<Slot>> NewListOfTimetables { get; private set; } = new List<List<Slot>>();
        public List<SubjectModel> SelectedSubjects { get; }
        public bool SlotSelectionIsChanged { get; private set; } = false;

        [Obsolete("This constructor is for initialization of XAML designer only")]
        public ChooseSpecificSlotModel() { }

        public ChooseSpecificSlotModel(List<SubjectModel> selectedSubjects ,
           Func<Slot[] , List<List<Slot>>> permutator) {
            SelectedSubjects = selectedSubjects;
            _permutator = permutator;
            for (int i = 0 ; i < selectedSubjects.Count ; i++) {
                _allSlots.AddRange(selectedSubjects[i].Slots);
            }
            GenerateSubjectSchemas(selectedSubjects);
        }

        private void GenerateSubjectSchemas(List<SubjectModel> selectedSubjects) {
            foreach (SubjectModel s in selectedSubjects) {
                _subjectSchemas.Add(new SubjectSchema(s.Slots));
            }
        }

        private void ToggleSlotSelection(int uid , bool isSelected) {
            SlotSelectionIsChanged = true;
            var matchingSlots = _allSlots.FindAll(x => x.UID == uid);
            for (int i = 0 ; i < matchingSlots.Count ; i++) {
                matchingSlots[i].IsSelected = isSelected;
            }
            NewListOfTimetables = _permutator.Invoke(_allSlots.FindAll(x => x.IsSelected).ToArray());
        }
        public void SelectSlot(int uid) {
            ToggleSlotSelection(uid , true);
        }

        public void DeselectSlot(int uid) {
            ToggleSlotSelection(uid , false);
        }

        public void SelectSlots(List<int> uids) {
            for (int i = 0 ; i < uids.Count ; i++) {
                SelectSlot(uids[i]);
            }
        }

        public void DeselectSlots(List<int> uids) {
            for (int i = 0 ; i < uids.Count ; i++) {
                DeselectSlot(uids[i]);
            }
        }

        public void CheckForError() {
            if (!SlotSelectionIsChanged) return;
            List<Slot> timetable = null;
            if (NewListOfTimetables != null) timetable = NewListOfTimetables[0];
            ErrorMessage = "";
            foreach (var schema in _subjectSchemas) {
                string message = schema.Validate(timetable);
                if (message != null) ErrorMessage += message;
            }
            GotError = ErrorMessage.Length > 0;
        }

        public bool GotError { get; private set; }
        public List<Slot> AllSlot => _allSlots;

        #region ViewProperties

        private string _errorMessage;
        public string ErrorMessage {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage , value);
        }

        #endregion
    }
}
