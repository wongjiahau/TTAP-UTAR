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
        private readonly List<string> _nameAndCodeOfAllSubjects = new List<string>();
        private readonly List<SubjectModel> _subjectModels;
        private readonly SubjectSelectionManager _subjectSelectionManager;

        public SubjectListModel() { }

        public SubjectListModel(List<SubjectModel> subjectModels, Func<Slot[], List<List<Slot>>> permutator = null,
                                ITaskRunnerWithProgressFeedback taskRunner = null) {
            _subjectModels = subjectModels;
            _subjectSelectionManager = new SubjectSelectionManager(subjectModels, permutator, taskRunner);
            _subjectSelectionManager.SelectedSubjectCountChanged +=
                _subjectSelectionManager_SelectedSubjectCountChanged;
            _subjectSelectionManager.NewListOfTimetablesGenerated +=
                _subjectSelectionManager_NewListOfTimetablesGenerated;
            foreach (var subjectModel in _subjectModels) {
                _nameAndCodeOfAllSubjects.Add(subjectModel.Name);
                _nameAndCodeOfAllSubjects.Add(subjectModel.Code);
            }
            _focusNavigator = new FocusNavigator(new List<IFocusable>(_subjectModels));
            _focusNavigator.FocusFirstItem();
        }

        public event EventHandler NewListOfTimetablesGenerated;

        private void _subjectSelectionManager_NewListOfTimetablesGenerated(object sender, EventArgs e) {
            NewListOfTimetablesGenerated?.Invoke(sender, null);
        }

        private void _subjectSelectionManager_SelectedSubjectCountChanged(object sender, EventArgs e) {
            SelectedSubjectCount = _subjectSelectionManager.SelectedSubjectCount;
        }

        public List<SubjectModel> ToList() {
            return _subjectModels;
        }

        public int NumberOfVisibleSubject() {
            return _subjectModels.Count(x => x.IsVisible);
        }

        #region SubjectSelection

        public void ToggleSelectionOnCurrentFocusedSubject() {
            _subjectSelectionManager.ToggleSelectionOnCurrentFocusedSubject();
        }

        /// <summary>
        /// Select a subject using code (e.g. MPU3113)
        /// </summary>
        /// <param name="subjectCode"></param>
        /// <param name="isSelectingSubject">Set this to false if you want to deselect the subject specified</param>
        public void SelectSubject(string subjectCode, bool isSelectingSubject = true) {
            var subjectToBeSelcted = _subjectModels.Find(x => x.Code == subjectCode);
            if (subjectToBeSelcted == null) throw new ArgumentException($"{subjectCode} does not match any subject.");
            subjectToBeSelcted.IsSelected = isSelectingSubject;
        }

        #endregion

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
                SetProperty(ref _displayMode, value);
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

        #region NumberOfSubjectSelectedProperty

        private int _selectedSubjectCount;

        public int SelectedSubjectCount {
            get => _selectedSubjectCount;
            set => SetProperty(ref _selectedSubjectCount, value);
        }

        #endregion

        #region SearchingProperties

        private bool _isHintLabelVisible = false;

        public bool IsHintLabelVisible {
            get => _isHintLabelVisible;
            set => SetProperty(ref _isHintLabelVisible, value);
        }

        private bool _isFeedbackPanelVisible = false;

        public bool IsFeedbackPanelVisible {
            get => _isFeedbackPanelVisible;
            set => SetProperty(ref _isFeedbackPanelVisible, value);
        }

        private bool _isErrorLabelVisible = false;

        public bool IsErrorLabelVisible {
            get => _isErrorLabelVisible;
            set => SetProperty(ref _isErrorLabelVisible, value);
        }

        private string _suggestedText;

        public string SuggestedText {
            get => _suggestedText;
            set => SetProperty(ref _suggestedText, value);
        }

        public void Search(string searchedText) {
            DisplayMode = DisplayModeEnum.DisplayAllSubject;
            IsHintLabelVisible = searchedText.Length > 0;
            IsErrorLabelVisible = IsFeedbackPanelVisible = false;
            if (SearchForMatchingSubjectAndDisplayThem(searchedText)) return;
            SuggestedText =
                LevenshteinDistance.GetClosestMatchingTerm(searchedText, _nameAndCodeOfAllSubjects.ToArray());
            if (SuggestedText != null) SearchForMatchingSubjectAndDisplayThem(SuggestedText.ToLower());
            IsFeedbackPanelVisible = SuggestedText != null;
            IsErrorLabelVisible = SuggestedText == null;
        }

        private bool SearchForMatchingSubjectAndDisplayThem(string searchedText) {
            bool somethingFound = false;
            var found = new List<SubjectModel>();
            var list = ToList();
            foreach (SubjectModel subject in list) {
                string comparedString = subject.Name.ToLower() + subject.Code.ToLower() +
                                        subject.Name.GetInitial().ToLower();
                if (comparedString.Contains(searchedText)) {
                    somethingFound = true;
                    subject.IsVisible = true;
                    found.Add(subject);
                    subject.HighlightedText = searchedText;
                }
                else subject.IsVisible = false;
            }
            _focusNavigator = new FocusNavigator(new List<IFocusable>(found));
            _focusNavigator.FocusFirstItem();
            return somethingFound;
        }

        #endregion

        #region NavigatingWithArrowKeys

        private FocusNavigator _focusNavigator;
        private ICommand _navigateToNextSubjectCommand;

        public ICommand NavigateToNextSubjectCommand
            => _navigateToNextSubjectCommand ??
               (_navigateToNextSubjectCommand = new RelayCommand(() => { _focusNavigator.NavigateToNext(); }));

        private ICommand _navigateToPreviousSubjectCommand;

        public ICommand NavigateToPreviousSubjectCommand
            => _navigateToPreviousSubjectCommand ??
               (_navigateToPreviousSubjectCommand = new RelayCommand(() => { _focusNavigator.NavigateToPrevious(); }));

        #endregion

        #endregion
    }
}