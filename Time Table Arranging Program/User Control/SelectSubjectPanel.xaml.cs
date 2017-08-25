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
using static System.Windows.Visibility;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    /// Interaction logic for SelectSubjectPanel.xaml
    /// </summary>
    public partial class SelectSubjectPanel : UserControl, INeedDataContext<List<SubjectModel>> {
        private List<ICheckBoxWithListDownMenu> _anyCheckBoxs;

        private List<string> _nameAndCodeOfAllSubjects;

        private string _suggestedText = "";

        public SelectSubjectPanel() {
            InitializeComponent();
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

        private ICheckBoxWithListDownMenu _lastCheckedSubject = new CheckBoxWithListDownMenu();
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
            _lastCheckedSubject = x;
            Update();
        }

        private void Update() {
            UpdateBottomPanelVisibility();
            SlotSelectionChanged(this , null);
            FocusSearchBox();
        }

        public void DeselectAndDisableLastSelectedSubject((SubjectModelWithState, SubjectModelWithState)? crashingSubjects) {
            _lastCheckedSubject.IsChecked = false;
            string errorMessage;
            if (crashingSubjects == null) errorMessage = "Cannot select this subject as it will cause clashes.";
            else {
                errorMessage = "Cannot select this subject as it clashes with ";
                if (crashingSubjects.Value.Item1.SubjectName == _lastCheckedSubject.SubjectName)
                    errorMessage += crashingSubjects.Value.Item2.SubjectName;
                else
                    errorMessage += crashingSubjects.Value.Item1.SubjectName;
            }
            _lastCheckedSubject.SetErrorMessage(errorMessage);
        }

        public void EnableAllDisabledSubject() {
            foreach (UIElement child in CheckerBoxStackPanel.Children) {
                if (child is ICheckBoxWithListDownMenu) {
                    (child as ICheckBoxWithListDownMenu).SetErrorMessage(null);
                }
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
            var found = new List<ICheckBoxWithListDownMenu>();
            foreach (UIElement child in CheckerBoxStackPanel.Children) {
                if (child is ICheckBoxWithListDownMenu) {
                    var target = child as ICheckBoxWithListDownMenu;
                    string comparedString = target.SubjectName.ToLower() + target.SubjectCode.ToLower();
                    if (comparedString.Contains(searchedText)) {
                        somethingFound = true;
                        child.Visibility = Visible;
                        found.Add(target);
                        (child as ICheckBoxWithListDownMenu).HighlightText = searchedText;
                    }
                    else {
                        child.Visibility = Collapsed;
                    }
                }
            }
            _iteratableList = new CyclicIteratableList<ICheckBoxWithListDownMenu>(found);
            _iteratableList.GetCurrent()?.Highlight();
            return somethingFound;
        }

        private void YesButton_OnClick(object sender , RoutedEventArgs e) {
            SearchBox.Text = _suggestedText;
        }

        private void SelectSubjectPanel_OnKeyDown(object sender , KeyEventArgs e) {
            if (SearchBox.IsKeyboardFocused() || SearchBox.IsFocused) return;
            FocusManager.SetFocusedElement(this , SearchBox);
        }



        private CyclicIteratableList<ICheckBoxWithListDownMenu> _iteratableList;
        public void SetDataContext(List<SubjectModel> subjectModel) {
            var subjectModels = subjectModel;
            _nameAndCodeOfAllSubjects = new List<string>();
            foreach (var subject in subjectModels) {
                _nameAndCodeOfAllSubjects.Add(subject.Name);
                _nameAndCodeOfAllSubjects.Add(subject.Code);
                var box = new CheckBoxWithListDownMenu();
                box.SetDataContext(subject);
                CheckerBoxStackPanel.Children.Add(box);
                box.Checked += Box_CheckChanged;
                box.ListViewCheckBox_Checked += Box_ListViewCheckBox_Checked;
            }
            _anyCheckBoxs =
                new List<ICheckBoxWithListDownMenu>(CheckerBoxStackPanel.Children.OfType<ICheckBoxWithListDownMenu>());
            _iteratableList = new CyclicIteratableList<ICheckBoxWithListDownMenu>(_anyCheckBoxs);
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
                    var current1 = (CheckBoxWithListDownMenu)_iteratableList.GetCurrent();
                    if (current1 == null) return;
                    current1.Highlight();
                    if (_iteratableList.AtLast()) ScrollViewer.ScrollToBottom();
                    else if (!current1.IsVisibleToUser(ScrollViewer)) ScrollViewer.PageUp();
                    break;
                case Key.Down:
                case Key.Right:
                    _iteratableList.GoToNext();
                    var current = (CheckBoxWithListDownMenu)_iteratableList.GetCurrent();
                    if (current == null) return;
                    current.Highlight();
                    if (_iteratableList.AtFirst()) ScrollViewer.ScrollToHome();
                    else if (!current.IsVisibleToUser(ScrollViewer)) ScrollViewer.PageDown();
                    break;
                case Key.Enter:
                    var current2 = (CheckBoxWithListDownMenu)_iteratableList.GetCurrent();
                    if (current2 == null) return;
                    current2.IsChecked = !_iteratableList.GetCurrent().IsChecked;
                    break;
                default: break;
            }
        }
    }
}