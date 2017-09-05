using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.AbstractClass;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.MVVM_Framework;

namespace Time_Table_Arranging_Program.User_Control.SubjectListFolder {
    public class SubjectListModel : ObservableObject {
        private List<SubjectModel> _subjectModels;
        private readonly List<string> _nameAndCodeOfAllSubjects = new List<string>();

        public SubjectListModel() { }

        public SubjectListModel(List<SubjectModel> subjectModels) {
            _subjectModels = subjectModels;
            foreach (var subjectModel in _subjectModels) {
                subjectModel.Selected += SubjectModel_Selected;
                subjectModel.Deselected += SubjectModel_Deselected;
                _nameAndCodeOfAllSubjects.Add(subjectModel.Name);
                _nameAndCodeOfAllSubjects.Add(subjectModel.Code);
            }
        }


        public List<SubjectModel> ToList() {
            return _subjectModels;
        }

        #region ViewModelProperties

        #region DisplayModeProperty

        public enum DisplayModeEnum {
            DisplayAllSubject,
            DisplaySelectedSubject
        }

        private DisplayModeEnum _displayMode;

        public DisplayModeEnum DisplayMode {
            get => _displayMode;
            set {
                SetProperty(ref _displayMode , value);
                switch (value) {
                    case DisplayModeEnum.DisplayAllSubject:
                        foreach (var subjectModel in _subjectModels) subjectModel.IsVisible = true;
                        break;
                    case DisplayModeEnum.DisplaySelectedSubject:
                        foreach (var subjectModel in _subjectModels) subjectModel.IsVisible = subjectModel.IsSelected;
                        break;
                }
            }
        }

        private ICommand _toggleDisplayModeCommand;
        public ICommand ToggleDisplayModeCommand {
            get {
                return _toggleDisplayModeCommand ??
                       (_toggleDisplayModeCommand = new RelayCommand(() => {
                           switch (DisplayMode) {
                               case DisplayModeEnum.DisplayAllSubject:
                                   DisplayMode = DisplayModeEnum.DisplaySelectedSubject;
                                   break;
                               case DisplayModeEnum.DisplaySelectedSubject:
                                   DisplayMode = DisplayModeEnum.DisplayAllSubject;
                                   break;
                           }
                       }));
            }
        }

        #endregion

        #region NoSubjectSelectedProperty

        private int _selectedSubjectCount;
        public int SelectedSubjectCount {
            get => _selectedSubjectCount;
            set => SetProperty(ref _selectedSubjectCount , value);
        }

        private void SubjectModel_Selected(object sender , EventArgs e) {
            SelectedSubjectCount++;
        }

        private void SubjectModel_Deselected(object sender , EventArgs e) {
            SelectedSubjectCount--;
        }

        #endregion


        #region SearchingProperties

        private CyclicIteratableList<SubjectModel> _iteratableList;
        private bool _somethingFound;
        public bool SomethingFound {
            get => _somethingFound;
            set => SetProperty(ref _somethingFound , value);
        }

        private string _suggestedText;
        public string SuggestedText {
            get => _suggestedText;
            set => SetProperty(ref _suggestedText , value);
        }

        public void Search(string searchedText) {
            SomethingFound = SearchForMatchingSubjectAndDisplayThem(searchedText);
            if (SomethingFound) return;
            SuggestedText = LevenshteinDistance.GetClosestMatchingTerm(searchedText , _nameAndCodeOfAllSubjects.ToArray());
            if (SuggestedText != null) SearchForMatchingSubjectAndDisplayThem(SuggestedText);
        }

        private bool SearchForMatchingSubjectAndDisplayThem(string searchedText) {
            bool somethingFound = false;
            var found = new List<SubjectModel>();
            var list = this.ToList();
            foreach (SubjectModel subject in list) {
                string comparedString = subject.Name.ToLower() + subject.Code.ToLower() + subject.Name.GetInitial().ToLower();
                if (comparedString.Contains(searchedText)) {
                    somethingFound = true;
                    subject.IsVisible = true;
                    found.Add(subject);
                    subject.HighlightedText = searchedText;
                }
                else {
                    subject.IsVisible = false;
                }
            }
            _iteratableList = new CyclicIteratableList<SubjectModel>(found);
            var current = _iteratableList.GetCurrent();
            if (current != null) current.IsFocused = true;
            return somethingFound;
        }
        #endregion
        #endregion
    }




}

