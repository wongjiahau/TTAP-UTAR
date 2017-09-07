using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Model;

namespace Time_Table_Arranging_Program.Class {
    public class SubjectSelectionManager {
        private readonly List<SubjectModel> _subjectModels;
        public int SelectedSubjectCount { get; private set; }
        public event EventHandler SubjectSelectionChanged;
        public SubjectSelectionManager(List<SubjectModel> subjectModels) {
            this._subjectModels = subjectModels;
            foreach (var subjectModel in _subjectModels) {
                subjectModel.Selected += SubjectModel_Selected;
                subjectModel.Deselected += SubjectModel_Deselected;
            }
        }

        public void ToggleSelectionOnCurrentFocusedSubject() {
            var current = _subjectModels.Find(x => x.IsFocused);
            current.IsSelected = !current.IsSelected;
            
        }

        private SubjectModel _currentlySelectedSubject;
        private void SubjectModel_Selected(object sender , EventArgs e) {
            SelectedSubjectCount++;
            SubjectSelectionChanged?.Invoke(this, null);
            _currentlySelectedSubject = (SubjectModel) sender;
        }

        private void SubjectModel_Deselected(object sender , EventArgs e) {
            SelectedSubjectCount--;
            SubjectSelectionChanged?.Invoke(this, null);
        }
    }
}
