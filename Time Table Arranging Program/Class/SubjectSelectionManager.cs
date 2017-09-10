using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.Pages;

namespace Time_Table_Arranging_Program.Class {
    public class SubjectSelectionManager {
        private readonly List<SubjectModel> _subjectModels;
        private readonly Func<Slot[] , List<List<Slot>>> _permutator;
        public int SelectedSubjectCount { get; private set; }
        public event EventHandler SubjectSelectionChanged;
        public event EventHandler NewListOfTimetablesGenerated;
        public SubjectSelectionManager(List<SubjectModel> subjectModels , Func<Slot[] , List<List<Slot>>> permutator) {
            this._subjectModels = subjectModels;
            _permutator = permutator;
            foreach (var subjectModel in _subjectModels) {
                subjectModel.Selected += SubjectModel_Selected;
                subjectModel.Deselected += SubjectModel_Deselected;
            }
        }

        public void ToggleSelectionOnCurrentFocusedSubject() {
            var current = _subjectModels.Find(x => x.IsFocused);
            current.IsSelected = !current.IsSelected;
        }

        private Slot[] GetSelectedSlots() {
            var result = new List<Slot>();
            for (int i = 0 ; i < _subjectModels.Count ; i++) {
                result.AddRange(_subjectModels[i].Slots); //Not using GetSelectedSlots here because this feature shall be moved to another place
            }
            return result.ToArray();
        }

        private SubjectModel _currentlySelectedSubject;
        private void SubjectModel_Selected(object sender , EventArgs e) {
            SelectedSubjectCount++;
            SubjectSelectionChanged?.Invoke(this , null);
            _currentlySelectedSubject = (SubjectModel)sender;
            var possibleTimetables = _permutator?.Invoke(GetSelectedSlots());
            if (possibleTimetables?.Count == 0) {
                var clashReport = new ClashFinder(_subjectModels, _permutator, _currentlySelectedSubject).GetReport();
                _currentlySelectedSubject.ClashReport = clashReport;
            }
            else {
                NewListOfTimetablesGenerated?.Invoke(possibleTimetables, null);
            }
        }

        private void SubjectModel_Deselected(object sender , EventArgs e) {
            SelectedSubjectCount--;
            SubjectSelectionChanged?.Invoke(this , null);
        }
    }
}
