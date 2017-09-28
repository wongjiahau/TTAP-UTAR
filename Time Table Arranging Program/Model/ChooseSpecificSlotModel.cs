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
        public List<SubjectModel> Subjects { get; }
        public bool IsSlotSelectionChanged { get; private set; } = false;

        [Obsolete("This constructor is for initialization of XAML designer only")]
        public ChooseSpecificSlotModel() { }

        public ChooseSpecificSlotModel(List<SubjectModel> subjects ,
           Func<Slot[] , List<List<Slot>>> permutator) {
            Subjects = subjects;
            _permutator = permutator;
            foreach (SubjectModel s in subjects) {
                _allSlots.AddRange(s.Slots);
                s.SlotSelectionChanged += SlotSelectionChanged;
            }
            GenerateSubjectSchemas(subjects);
        }

        private void SlotSelectionChanged(object sender , EventArgs e) {
            IsSlotSelectionChanged = true;
            NewListOfTimetables = _permutator.Invoke(_allSlots.FindAll(x => x.IsSelected).ToArray());
        }

        private void GenerateSubjectSchemas(List<SubjectModel> selectedSubjects) {
            foreach (SubjectModel s in selectedSubjects) {
                _subjectSchemas.Add(new SubjectSchema(s.Slots));
            }
        }

        public void CheckForError() {
            if (!IsSlotSelectionChanged) return;
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
