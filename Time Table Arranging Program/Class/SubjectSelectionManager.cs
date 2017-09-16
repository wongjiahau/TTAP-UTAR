using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.Pages;
using Time_Table_Arranging_Program.User_Control.CheckboxWithListDownMenuFolder.ErrorMessageType;

namespace Time_Table_Arranging_Program.Class {
    public class SubjectSelectionManager {
        private readonly List<SubjectModel> _disabledSubjects = new List<SubjectModel>();
        private readonly Func<Slot[] , List<List<Slot>>> _permutator;
        private readonly List<SubjectModel> _subjectModels;
        private readonly ITaskRunnerWithProgressFeedback _taskRunner;


        public SubjectSelectionManager(List<SubjectModel> subjectModels , Func<Slot[] , List<List<Slot>>> permutator ,
                                       ITaskRunnerWithProgressFeedback taskRunner) {
            _subjectModels = subjectModels;
            _permutator = permutator;
            _taskRunner = taskRunner;
            foreach (var subjectModel in _subjectModels) {
                subjectModel.Selected += SubjectModel_Selected;
                subjectModel.Deselected += SubjectModel_Deselected;
            }
        }

        private int _selectedSubjectCount;
        public int SelectedSubjectCount {
            get => _selectedSubjectCount;
            private set {
                _selectedSubjectCount = value;
                SelectedSubjectCountChanged?.Invoke(this , null);
            }
        }

        public event EventHandler SelectedSubjectCountChanged;
        public event EventHandler NewListOfTimetablesGenerated;

        public void ToggleSelectionOnCurrentFocusedSubject() {
            var current = _subjectModels.Find(x => x.IsFocused);
            if (current == null) return;
            current.IsSelected = !current.IsSelected;
        }

        private List<List<Slot>> GetPossibleTimetables() {
            var possibleTimetables = _permutator?.Invoke(GetSelectedSlots());
            return possibleTimetables;

            Slot[] GetSelectedSlots()
            {
                var result = new List<Slot>();
                for (int i = 0 ; i < _subjectModels.Count ; i++) {
                    if (_subjectModels[i].IsSelected)
                        result.AddRange(_subjectModels[i]
                            .Slots); //Not using GetSelectedSlots here because this feature shall be moved to another place
                }
                return result.ToArray();
            }
        }

        private void SubjectModel_Selected(object sender , EventArgs e) {
            var currentlySelectedSubject = (SubjectModel)sender;
            var possibleTimetables = new List<List<Slot>>();
            _taskRunner.RunTask(() => possibleTimetables = GetPossibleTimetables());
            if (possibleTimetables?.Count == 0) {
                DisableClashingSubject(currentlySelectedSubject);
            }
            else {
                SelectedSubjectCount++;
                NewListOfTimetablesGenerated?.Invoke(possibleTimetables , null);
            }
        }

        private void DisableClashingSubject(SubjectModel currentlySelectedSubject) {
            var clashReport =
                new ClashFinder(_subjectModels, _permutator, currentlySelectedSubject).GetReport();
            _disabledSubjects.Add(currentlySelectedSubject);
            currentlySelectedSubject.ClashReport = clashReport;
        }

        private void SubjectModel_Deselected(object sender , EventArgs e) {
            var currentlyDeselectedSubject = (SubjectModel)sender;
            if (_disabledSubjects.Any(x => x.Code == currentlyDeselectedSubject.Code)) return;
            EnableClashedSubjectIfPossible(currentlyDeselectedSubject);
            SelectedSubjectCount--;
            var possibleTimeatables = new List<List<Slot>>();
            _taskRunner.RunTask(() => possibleTimeatables = GetPossibleTimetables());
            NewListOfTimetablesGenerated?.Invoke(possibleTimeatables , null);
        }

        private void EnableClashedSubjectIfPossible(SubjectModel currentlyDeselectedSubject) {
            for (var i = 0 ; i < _disabledSubjects.Count ; i++) {
                SubjectModel s = _disabledSubjects[i];
                switch (s.ClashingErrorType) {
                    case ClashingErrorType.SingleClashingError:
                        if (s.ClashingCounterparts.Any(x => x.Code == currentlyDeselectedSubject.Code))
                            goto default;
                        else break;
                    case ClashingErrorType.GroupClashingError:
                        if (!StillGroupClashWithCurrentlySelectedSubject(s)) goto default;
                        else break;
                    default:
                        s.ClashReport = new NullClashReport();
                        _disabledSubjects.Remove(s);
                        i--;
                        break;
                }
            }
        }

        private bool StillGroupClashWithCurrentlySelectedSubject(SubjectModel currentlySelectedSubject) {
            var list = new List<Slot>();
            list.AddRange(currentlySelectedSubject.GetSelectedSlots());
            list.AddRange(_subjectModels.GetSelectedSlots());
            return _permutator?.Invoke(list.ToArray()).Count == 0;
        }
    }
}