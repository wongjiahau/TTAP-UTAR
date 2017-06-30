using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    /// Interaction logic for SelectSubjectPanel.xaml
    /// </summary>
    public partial class SelectSubjectPanel : UserControl {
        private List<ICheckBoxWithListDownMenu> _anyCheckBoxs;

        private string[] _nameAndCodeOfAllSubjects;

        private string _suggestedText = "";

        public SelectSubjectPanel() {
            InitializeComponent();
        }


        public HashSet<int> UIDofSelectedSlots { get; } = new HashSet<int>();
        public event EventHandler SlotSelectionChanged;


        public void Clear() {
            CheckerBoxStackPanel.Children.Clear();
        }

        

        public void CreateCheckBoxes(SlotList inputSlots) {
            var subjects = inputSlots.GetNamesOfAllSubjects();
            var codes = inputSlots.GetCodesOfAllSubjects();
            _nameAndCodeOfAllSubjects = subjects.Concat(codes).ToArray();
            for (var i = 0; i < subjects.Length; i++) {
                ICheckBoxWithListDownMenu box = new CheckBoxWithListDownMenu();
                box.SubjectName = subjects[i];
                box.SubjectCode = codes[i];
                if (inputSlots.SelectedSubjectNames.Any(s => s == subjects[i])) {
                    box.IsChecked = true;
                    box.FontWeight = FontWeights.Bold;
                }
                box.Checked += Box_CheckChanged;
                box.SlotList = inputSlots.GetSlotsOf(subjects[i]);
                box.ListViewCheckBox_Checked += Box_ListViewCheckBox_Checked;
                CheckerBoxStackPanel.Children.Add((CheckBoxWithListDownMenu) box);
            }
            _anyCheckBoxs =
                new List<ICheckBoxWithListDownMenu>(CheckerBoxStackPanel.Children.OfType<ICheckBoxWithListDownMenu>());
        }

        public string[] GetNamesOfCheckedSubject() {
            var checkedSubject = new List<string>();
            foreach (var box in _anyCheckBoxs) {
                if (box.IsChecked) checkedSubject.Add(box.SubjectName);
            }
            return checkedSubject.ToArray();
        }

        private void Box_ListViewCheckBox_Checked(object sender, RoutedEventArgs e) {
            var c = (ICheckBoxWithListDownMenu) sender;
            foreach (var uid in c.UIDofSelectedSlots) {
                UIDofSelectedSlots.Add(uid);
            }
            foreach (var uid in c.UIDofDeselectedSlots) {
                UIDofSelectedSlots.Remove(uid);
            }
            Update();
        }

        private void Update() {
            if (UIDofSelectedSlots.Count == 0) {
                ToggleViewButton.IsEnabled = false;
                ToggleViewButton.IsChecked = false;
            }
            else {
                ToggleViewButton.IsEnabled = true;
            }
            SlotSelectionChanged(this, null);
            Dispatcher.BeginInvoke(
                DispatcherPriority.Input,
                new Action(delegate
                {
                    FocusManager.SetFocusedElement(this, SearchBox);
                    IInputElement focusedElement = FocusManager.GetFocusedElement(this);
                }));
        }

        private void Box_CheckChanged(object sender, RoutedEventArgs e) {
            var x = sender as ICheckBoxWithListDownMenu;
            if (x.IsChecked) {
                x.FontWeight = FontWeights.Bold;
                UIDofSelectedSlots.UnionWith(x.UIDofSelectedSlots);
            }
            else {
                x.FontWeight = FontWeights.Normal;
                UIDofSelectedSlots.ExceptWith(x.UIDofSelectedSlots.Union(x.UIDofDeselectedSlots));
            }
            e.Handled = true;
            Update();
        }

        private void ToggleViewButton_OnChecked(object sender, RoutedEventArgs e) {
            foreach (UIElement child in CheckerBoxStackPanel.Children) {
                if (child is ICheckBoxWithListDownMenu) {
                    child.Visibility = 
                        (child as ICheckBoxWithListDownMenu).IsChecked ? 
                        Visibility.Visible : 
                        Visibility.Collapsed;
                }
            }
            ToggleViewButton.ToolTip = "Show all subject";
        }

        private void ToggleViewButton_OnUnchecked(object sender, RoutedEventArgs e) {
            foreach (UIElement child in CheckerBoxStackPanel.Children) {
                if (child is ICheckBoxWithListDownMenu) {
                    child.Visibility = Visibility.Visible;
                }
            }
            ToggleViewButton.ToolTip = "Show selected subjects";
        }

        private void ViewChangerButton_OnClick(object sender, RoutedEventArgs e) {
            if (ViewChangerButton.Content.ToString() == "Show selected subjects") {
                foreach (UIElement child in CheckerBoxStackPanel.Children) {
                    if (child is ICheckBoxWithListDownMenu) {
                        child.Visibility =
                            (child as ICheckBoxWithListDownMenu).IsChecked
                                ? Visibility.Visible
                                : Visibility.Collapsed;
                    }
                }
                ViewChangerButton.Content = "Show all subjects";
                SearchBox.Visibility = Visibility.Collapsed;
            }
            else {
                foreach (UIElement child in CheckerBoxStackPanel.Children) {
                    if (child is ICheckBoxWithListDownMenu) {
                        child.Visibility = Visibility.Visible;
                    }
                }
                ViewChangerButton.Content = "Show selected subjects";
                SearchBox.Visibility = Visibility.Visible;
            }
        }

        private void SearchBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs) {
            string searchedText = SearchBox.Text.ToLower();
            bool somethingFound = SearchForMatchingSubjectAndDisplayThem(searchedText);
            if (somethingFound) {
                FeedbackPanel.Visibility = Visibility.Collapsed;
                ErrorLabel.Visibility = Visibility.Collapsed;
            }
            else {
                _suggestedText = LevenshteinDistance.GetClosestMatchingTerm(searchedText, _nameAndCodeOfAllSubjects);
                if (_suggestedText == null) {
                    FeedbackPanel.Visibility = Visibility.Collapsed;
                    ErrorLabel.Text = "No result found . . .";
                    ErrorLabel.Visibility = Visibility.Visible;
                }
                else {
                    ErrorLabel.Visibility = Visibility.Collapsed;
                    FeedbackPanel.Visibility = Visibility.Visible;
                    SuggestedTextLabel.Text = _suggestedText.Beautify();
                    SearchForMatchingSubjectAndDisplayThem(_suggestedText.ToLower());
                }
            }
        }

        private bool SearchForMatchingSubjectAndDisplayThem(string searchedText) {
            bool somethingFound = false;
            foreach (UIElement child in CheckerBoxStackPanel.Children) {
                if (child is ICheckBoxWithListDownMenu) {
                    var target = child as ICheckBoxWithListDownMenu;
                    string comparedString = target.SubjectName.ToLower() + target.SubjectCode.ToLower();
                    if (comparedString.Contains(searchedText)) {
                        somethingFound = true;
                        child.Visibility = Visibility.Visible;
                        (child as ICheckBoxWithListDownMenu).HighlightText = searchedText;
                    }
                    else {
                        child.Visibility = Visibility.Collapsed;
                    }
                }
            }
            return somethingFound;
        }

        private void SearchBox_OnGotFocus(object sender, RoutedEventArgs e) {
            ToggleViewButton.IsChecked = false;
        }

        private void YesButton_OnClick(object sender, RoutedEventArgs e) {
            SearchBox.Text = _suggestedText;
        }

        private void SelectSubjectPanel_OnKeyDown(object sender, KeyEventArgs e) {
            if (SearchBox.IsKeyboardFocused() || SearchBox.IsFocused) return;
            //if (e.Key == Key.Back) {
            //    string s = SearchBox.Text;
            //    SearchBox.Text = s.Substring(0, s.Length - 1);
            //    return;
            //}
            //SearchBox.Text += e.Key;
            //SearchBox.TextBox.CaretIndex = 1;
            FocusManager.SetFocusedElement(this, SearchBox);
            // SearchBox.Focus();
        }
    }
}