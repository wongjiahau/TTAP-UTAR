using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Helper;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.Pages;
using Time_Table_Arranging_Program.UserInterface;
using Time_Table_Arranging_Program.User_Control.CheckboxWithListDownMenuFolder;
using Time_Table_Arranging_Program.User_Control.CheckboxWithListDownMenuFolder.ErrorMessageType;
using static System.Windows.Visibility;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    /// Interaction logic for SelectSubjectPanel.xaml
    /// </summary>
    public partial class SelectSubjectPanel : UserControl, INeedDataContext<List<SubjectModel>> {
        private List<ICheckBoxWithListDownMenu> _anyCheckBoxs;
        private List<string> _nameAndCodeOfAllSubjects;
        private string _suggestedText = "";
        private Func<Slot[] , List<List<Slot>>> _permutator;
        public SelectSubjectPanel() {
            InitializeComponent();
        }

        public void Initialize(Func<Slot[] , List<List<Slot>>> permutator , List<SubjectModel> subjectModels) {
            _permutator = permutator;
            SetDataContext(subjectModels);
        }
        public void SetDrawerHost(DrawerHost drawerHost) {
            _drawerHost = drawerHost;

        }

        public HashSet<int> UIDofSelectedSlots { get; } = new HashSet<int>();
        public event EventHandler SlotSelectionChanged;

        public void Clear() {
            CheckerBoxStackPanel.Children.Clear();
        }

        public string[] GetNamesOfCheckedSubject() {
            var checkedSubject = new List<string>();
            foreach (var box in _anyCheckBoxs) {
                if (box.IsChecked) checkedSubject.Add(box.SubjectName);
            }
            return checkedSubject.ToArray();
        }

        private void Box_ListViewCheckBox_Checked(object sender , RoutedEventArgs e) {
            var c = (ICheckBoxWithListDownMenu)sender;
            foreach (var uid in c.UIDofSelectedSlots) {
                UIDofSelectedSlots.Add(uid);
            }
            foreach (var uid in c.UIDofDeselectedSlots) {
                UIDofSelectedSlots.Remove(uid);
            }
            Update();
        }

        private ICheckBoxWithListDownMenu _lastClickedSubject = new CheckboxWithListDownMenuFolder.CheckBoxWithListDownMenu();
        private void Box_CheckChanged(object sender , RoutedEventArgs e) {
            var x = sender as ICheckBoxWithListDownMenu;
            if (x.IsChecked) {
                x.FontWeight = FontWeights.Bold;
                UIDofSelectedSlots.UnionWith(x.UIDofSelectedSlots);
            }
            else {
                x.FontWeight = FontWeights.Normal;
                UIDofSelectedSlots.ExceptWith(x.UIDofSelectedSlots.Union(x.UIDofDeselectedSlots));
            }
            //e.Handled = true;
            _lastClickedSubject = x;
            Update();
        }

        private void Update() {
            UpdateBottomPanelVisibility();
            SlotSelectionChanged(this , null);
            FocusSearchBox();
        }

        public void EnableRelevantDisabledSubject() {
            foreach (UIElement child in CheckerBoxStackPanel.Children) {
                if (!(child is ICheckBoxWithListDownMenu)) continue;
                var x = (child as ICheckBoxWithListDownMenu);
                if (x.IsSelectable) continue;
                if (x.NameOfClashingCounterpart == null ||
                    x.NameOfClashingCounterpart == _lastClickedSubject.SubjectName) ;
            }
        }

        public void FocusSearchBox() {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Input ,
                new Action(delegate {
                    FocusManager.SetFocusedElement(this , SearchBox);
                    IInputElement focusedElement = FocusManager.GetFocusedElement(this);
                }));

        }

        private void UpdateBottomPanelVisibility() {
            DoubleAnimation da;
            if (UIDofSelectedSlots.Count == 0) {
                da = CustomAnimation.GetLeavingScreenAnimation(70 , 0 , false);
                ViewChanger.Badge = null;
                if (ViewChangerButton.Content.ToString() == "Show all subjects") return;
            }
            else {
                da = CustomAnimation.GetEnteringScreenAnimation(0 , 70 , false);
                ViewChanger.Badge = GetNamesOfCheckedSubject().Length;
                if (BottomPanel.ActualHeight > 0) return;
            }
            BottomPanel.BeginAnimation(HeightProperty , da);
        }

        private void ViewChangerButton_OnClick(object sender , RoutedEventArgs e) {
            if (ViewChangerButton.Content.ToString() == "Show selected subjects") {
                ShowSelectedSubjects();
            }
            else {
                ShowAllSubjects();
            }
            UpdateBottomPanelVisibility();
        }

        private void ShowAllSubjects() {
            foreach (UIElement child in CheckerBoxStackPanel.Children) {
                if (child is ICheckBoxWithListDownMenu) {
                    child.Visibility = Visible;
                }
            }
            ViewChangerButton.Content = "Show selected subjects";
        }

        private void ShowSelectedSubjects() {
            foreach (UIElement child in CheckerBoxStackPanel.Children) {
                if (child is ICheckBoxWithListDownMenu) {
                    child.Visibility =
                        (child as ICheckBoxWithListDownMenu).IsChecked
                            ? Visible
                            : Collapsed;
                }
            }
            ViewChangerButton.Content = "Show all subjects";
        }

        #region SearchBoxEvents
        private void SearchBoxOnTextChanged(object sender , TextChangedEventArgs textChangedEventArgs) {
            ShowAllSubjects();
            string searchedText = SearchBox.Text.ToLower();
            HintLabel.Visibility = searchedText == "" ? Collapsed : Visible;
            bool somethingFound = SearchForMatchingSubjectAndDisplayThem(searchedText);
            if (somethingFound) {
                FeedbackPanel.Visibility = Collapsed;
                ErrorLabel.Visibility = Collapsed;
            }
            else {
                _suggestedText = LevenshteinDistance.GetClosestMatchingTerm(searchedText , _nameAndCodeOfAllSubjects.ToArray());
                if (_suggestedText == null) {
                    FeedbackPanel.Visibility = Collapsed;
                    ErrorLabel.Text = "No result found . . .";
                    ErrorLabel.Visibility = Visible;
                }
                else {
                    ErrorLabel.Visibility = Collapsed;
                    FeedbackPanel.Visibility = Visible;
                    SuggestedTextLabel.Text = _suggestedText.Beautify();
                    SearchForMatchingSubjectAndDisplayThem(_suggestedText.ToLower());
                }
            }
        }

        private bool SearchForMatchingSubjectAndDisplayThem(string searchedText) {
            bool somethingFound = false;
            var found = new List<SubjectModel>();
            foreach (SubjectModel subject in _subjectModels) {
                string comparedString = subject.Name.ToLower() + subject.Code.ToLower();
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

        private void YesButton_OnClick(object sender , RoutedEventArgs e) {
            SearchBox.Text = _suggestedText;
        }

        #endregion
        private void SelectSubjectPanel_OnKeyDown(object sender , KeyEventArgs e) {
            if (SearchBox.IsKeyboardFocused() || SearchBox.IsFocused) return;
            FocusManager.SetFocusedElement(this , SearchBox);
        }



        private CyclicIteratableList<SubjectModel> _iteratableList;
        private List<SubjectModel> _subjectModels;
        private List<SubjectModel> _previousSelectedSubjects = new List<SubjectModel>();
        public void SetDataContext(List<SubjectModel> subjectModels) {
            _subjectModels = subjectModels;
            _nameAndCodeOfAllSubjects = new List<string>();
            foreach (var subject in _subjectModels) {
                _nameAndCodeOfAllSubjects.Add(subject.Name);
                _nameAndCodeOfAllSubjects.Add(subject.Code);
                subject.Selected += Subject_Selected;
                subject.Deselected += Subject_Deselected;
                var box = new CheckBoxWithListDownMenu();
                box.SetDataContext(subject);
                CheckerBoxStackPanel.Children.Add(box);
                //box.Checked += Box_CheckChanged;
                box.ListViewCheckBox_Checked += Box_ListViewCheckBox_Checked;
            }
            _anyCheckBoxs =
                new List<ICheckBoxWithListDownMenu>(CheckerBoxStackPanel.Children.OfType<ICheckBoxWithListDownMenu>());
            _iteratableList = new CyclicIteratableList<SubjectModel>(_subjectModels);
        }

        private void Subject_Deselected(object sender , EventArgs e) {

        }

        public List<List<Slot>> PossibleTimetables;
        private void Subject_Selected(object sender , EventArgs e) {
            var currentlySelectedSubject = sender as SubjectModel;
            var prototype = new List<SubjectModel> { currentlySelectedSubject };
            prototype.AddRange(_previousSelectedSubjects);
            PossibleTimetables = _permutator.Invoke(prototype.GetSelectedSlots().ToArray());
            if (PossibleTimetables == null || PossibleTimetables.Count == 0) {
                var clashingSubjects = new ClashFinder(_subjectModels , _permutator).CrashingSlots;
                if (clashingSubjects == null)
                    currentlySelectedSubject.ClashingErrorType = ClashingErrorType.GroupClashingError;
                else {
                    currentlySelectedSubject.NameOfCrashingCounterpart =
                    clashingSubjects.Value.Item1.SubjectName == _lastClickedSubject.SubjectName ?
                    clashingSubjects.Value.Item2.SubjectName :
                    clashingSubjects.Value.Item1.SubjectName;
                    currentlySelectedSubject.ClashingErrorType = ClashingErrorType.SingleClashingError;
                }
            }
            else {
                _previousSelectedSubjects.Add(currentlySelectedSubject);
                SlotSelectionChanged?.Invoke(this , null);
            }
        }

        private void DoneButton_OnClick(object sender , RoutedEventArgs e) {
            DrawerHost d = this._drawerHost;
            d.IsLeftDrawerOpen = false;
            Global.Snackbar.MessageQueue.Enqueue("Click 'Set time constraint' button.");
        }

        private DrawerHost _drawerHost;

        private void SearchBox_OnOnKeyPressed(object sender , KeyEventArgs e) {
            switch (e.Key) {
                case Key.Up:
                case Key.Left:
                    _iteratableList.GoToPrevious();
                    var current1 = _iteratableList.GetCurrent();
                    if (current1 == null) return;
                    current1.IsFocused = true;
                    if (_iteratableList.AtLast()) ScrollViewer.ScrollToBottom();
                    //else if (!current1.IsVisibleToUser(ScrollViewer)) ScrollViewer.PageUp();
                    break;
                case Key.Down:
                case Key.Right:
                    _iteratableList.GoToNext();
                    var current = _iteratableList.GetCurrent();
                    if (current == null) return;
                    current.IsFocused = true;
                    if (_iteratableList.AtFirst()) ScrollViewer.ScrollToHome();
                    //else if (!current.IsVisibleToUser(ScrollViewer)) ScrollViewer.PageDown();
                    break;
                case Key.Enter:
                    var current2 = _iteratableList.GetCurrent();
                    if (current2 == null) return;
                    current2.IsSelected = !current2.IsSelected;
                    break;
                default: break;
            }
        }
    }
}