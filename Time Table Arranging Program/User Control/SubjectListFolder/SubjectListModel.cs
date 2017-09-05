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
            _focusNavigator = new FocusNavigator(new List<IFocusable>(_subjectModels));
            _focusNavigator.FocusFirstItem();
        }


        public List<SubjectModel> ToList() {
            return _subjectModels;
        }

        public int NumberOfVisibleSubject() {
            return _subjectModels.Count(x => x.IsVisible);
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
        public ICommand ToggleDisplayModeCommand
            => _toggleDisplayModeCommand ??
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
        private bool _isHintLabelVisible = false;
        public bool IsHintLabelVisible {
            get => _isHintLabelVisible;
            set => SetProperty(ref _isHintLabelVisible , value);
        }

        private bool _isFeedbackPanelVisible = false;
        public bool IsFeedbackPanelVisible {
            get => _isFeedbackPanelVisible;
            set => SetProperty(ref _isFeedbackPanelVisible , value);
        }

        private bool _isErrorLabelVisible = false;
        public bool IsErrorLabelVisible {
            get => _isErrorLabelVisible;
            set => SetProperty(ref _isErrorLabelVisible , value);
        }


        private string _suggestedText;
        public string SuggestedText {
            get => _suggestedText;
            set => SetProperty(ref _suggestedText , value);
        }

        public void Search(string searchedText) {
            DisplayMode = DisplayModeEnum.DisplayAllSubject;
            IsHintLabelVisible = searchedText.Length > 0;
            IsErrorLabelVisible = IsFeedbackPanelVisible = false;
            var somethingFound = SearchForMatchingSubjectAndDisplayThem(searchedText);
            if (somethingFound) return;
            SuggestedText = LevenshteinDistance.GetClosestMatchingTerm(searchedText , _nameAndCodeOfAllSubjects.ToArray());
            if (SuggestedText != null) SearchForMatchingSubjectAndDisplayThem(SuggestedText.ToLower());
            IsFeedbackPanelVisible = SuggestedText != null;
            IsErrorLabelVisible = SuggestedText == null;
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
                else subject.IsVisible = false;
            }
            _focusNavigator = new FocusNavigator(new List<IFocusable>(found));
            return somethingFound;
        }
        #endregion

        #region NavigatingWithArrowKeys
        private FocusNavigator _focusNavigator;
        private ICommand _navigateToNextSubjectCommand;
        public ICommand NavigateToNextSubjectCommand
            => _navigateToNextSubjectCommand ?? 
                (_navigateToNextSubjectCommand = new RelayCommand(() => {
                    _focusNavigator.NavigateToNext();                    
                }));

        private ICommand _navigateToPreviousSubjectCommand;
        public ICommand NavigateToPreviousSubjectCommand
            => _navigateToPreviousSubjectCommand ?? 
                (_navigateToPreviousSubjectCommand = new RelayCommand(() => {
                    _focusNavigator.NavigateToPrevious();                    
                }));
        #endregion

        #endregion
    }




}

