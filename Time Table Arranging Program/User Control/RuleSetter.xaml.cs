using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.UserInterface;

namespace Time_Table_Arranging_Program {
    /// <summary>
    ///     Interaction logic for RuleSetter.xaml
    /// </summary>
    public interface IRuleSetter {
        bool IsChecked { get; }
        Predicate<Slot> GetRulePredicate();
        event EventHandler xButton_Clicked;
    }

    public partial class RuleSetter : UserControl, IRuleSetter {
        private string _day;
        private ITime _time1;
        private ITime _time2;

        private double _widthOfTimeChooser;
        private double _widthOfToLabel;

        public RuleSetter() {
            InitializeComponent();
        }

        public Predicate<Slot> GetRulePredicate() {
            _day = (DayCombobox.SelectedItem as ComboBoxItem).Content as string;
            var x = (PredicateCombobox.SelectedItem as ComboBoxItem).Content as string;
            _time1 = TimeChooser1?.GetChosenTime();
            _time2 = TimeChooser2?.GetChosenTime();
            if (x == "All day") {
                return AllDayPredicate;
            }
            if (x == "Before") {
                return BeforePredicate;
            }
            if (x == "After") {
                return AfterPredicate;
            }
            if (x == "Between") {
                return BetweenPredicate;
            }
            return null;
        }

        public event EventHandler xButton_Clicked;

        public bool IsChecked {
            get { return ToggleButton.IsChecked.Value; }
        }

        private void PredicateCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            var selectedText = (PredicateCombobox.SelectedItem as ComboBoxItem).Content as string;

            if (selectedText == "All day") {
                ToggleView(TimeChooser1, false);
                ToggleView(ToLabel, false);
                ToggleView(TimeChooser2, false);
            }
            else if (selectedText == "Before" || selectedText == "After") {
                ToggleView(TimeChooser1, true);
                ToggleView(ToLabel, false);
                ToggleView(TimeChooser2, false);
            }
            else if (selectedText == "Between") {
                ToggleView(TimeChooser1, true);
                ToggleView(ToLabel, true);
                ToggleView(TimeChooser2, true);
            }
        }

        private void ToggleView(Control ui, bool isShow) {
            DoubleAnimation animation;
            if (isShow) {
                if (ui.ActualWidth != 0) return;
                animation = CustomAnimation.GetEnteringScreenAnimation(0,
                    ui is Label ? _widthOfToLabel : _widthOfTimeChooser, false);
                ui.BeginAnimation(WidthProperty, animation);
            }
            else {
                if (ui.ActualWidth == 0) return;
                animation =
                    CustomAnimation.GetLeavingScreenAnimation(ui is Label ? _widthOfToLabel : _widthOfTimeChooser, 0,
                        false);
                ui.BeginAnimation(WidthProperty, animation);
            }
        }

        private bool AllDayPredicate(Slot s) {
            var d = Day.Parse(_day);
            return s.Day.Equals(d);
            //return s.Day == _day;
        }

        private bool BeforePredicate(Slot s) {
            return s.StartTime.LessThan(_time1) && AllDayPredicate(s);
        }

        private bool AfterPredicate(Slot s) {
            return s.EndTime.MoreThan(_time1) && AllDayPredicate(s);
        }

        private bool BetweenPredicate(Slot s) {
            var timePeriod = new TimePeriod(_time1, _time2);
            return
                s.TimePeriod.IntersectWith(timePeriod)
                &&
                AllDayPredicate(s);
        }

        private void XButton_OnClick(object sender, RoutedEventArgs e) {
            xButton_Clicked(this, null);
        }

        private void RuleSetter_OnLoaded(object sender, RoutedEventArgs e) {
            _widthOfTimeChooser = TimeChooser1.ActualWidth;
            _widthOfToLabel = ToLabel.ActualWidth;
            TimeChooser1.Width = 0;
            TimeChooser2.Width = 0;
            ToLabel.Width = 0;
        }

        private void ToggleButton_OnClick(object sender, RoutedEventArgs e) {
            var b = sender as ToggleButton;
            StackPanel.IsEnabled = b.IsChecked.Value;
        }
    }
}