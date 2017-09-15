using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.StateSummary;
using Time_Table_Arranging_Program.User_Control;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Pages {
    /// <summary>
    /// Interaction logic for Page_StateSummary.xaml
    /// </summary>
    public partial class Window_StateSummary : Window {
        private List<Slot> _inputSlots;
        private List<List<Slot>> _outputTimetables;
        private readonly List<Predicate<Slot>> _predicates = new List<Predicate<Slot>>();
        private readonly IStateElementFactory _factory;
        public Window_StateSummary(List<Slot> inputSlots , List<List<Slot>> outputTimetables) {
            _inputSlots = inputSlots;
            _outputTimetables = outputTimetables;
            _factory = new RealStateElementFactory(Checked , Unchecked);
            InitializeComponent();
            TimeTableGui.GenerateStateSummary(_outputTimetables , _factory);
        }

        private void RegenerateOutput() {
            var package = new TimetablesAndPredicates(_outputTimetables , Predicates);
            var bg = CustomBackgroundWorker<TimetablesAndPredicates , List<List<Slot>>>.
            RunAndShowLoadingScreen(Filterer.Filter , package ,
                "Filtering unsatisfactory timetables . . .");
            var filteredTimetable = bg.GetResult();
            int removedCount = _outputTimetables.Count - filteredTimetable.Count;
            int remainingCount = _outputTimetables.Count - removedCount;
            Label1.Content = $"Removed {removedCount} unsatisfactory timetables.";
            Label2.Content = $"{remainingCount} timetables remaining.";
            RemainingTimetables = filteredTimetable;
            TimeTableGui.RegenerateStateSummary(filteredTimetable , _factory);
        }

        private void Unchecked(Predicate<Slot> predicate) {
            _predicates.Remove(predicate);
            RegenerateOutput();
        }
        private void Checked(Predicate<Slot> predicate) {
            _predicates.Add(predicate);
            RegenerateOutput();
        }

        public List<Predicate<Slot>> Predicates => _predicates;
        public bool UserClickedDone { get; private set; } = false;
        public List<List<Slot>> RemainingTimetables { get; private set; }

        private void DoneButton_OnClick(object sender , RoutedEventArgs e) {
            UserClickedDone = true;
            this.Hide();
        }

        private void BackButton_OnClick(object sender , RoutedEventArgs e) {
            UserClickedDone = false;
            this.Hide();
        }


        private void Window_StateSummary_OnLoaded(object sender , RoutedEventArgs e) {
            UserClickedDone = false;
        }

        private void HelpButton_OnClick(object sender , RoutedEventArgs e) {
            MyMessageBox.ShowOk(
                "How it works?" ,
                "It works by searching through all the possible combination and check for which time period definitely have class and vice versa."

                );
            //DialogHost.IsOpen = true;                            
        }


    }

    public delegate void ModifyPredicateListDelegate(Predicate<Slot> predicate);
    public interface IStateElementFactory {
        UIElement CreateDefinitelyOccupiedState();
        UIElement CreateMaybeUnoccupiedState(IStateCell cell , ModifyPredicateListDelegate checkedAction = null , ModifyPredicateListDelegate uncheckedAction = null);
        UIElement CreateDefinitelyUnoccupiedState();
    }

    public class MockStateElementFactory : IStateElementFactory {
        public UIElement CreateDefinitelyOccupiedState() {
            return new Border() { Background = Brushes.DarkRed , Height = 50 };
        }

        public virtual UIElement CreateMaybeUnoccupiedState(IStateCell cell , ModifyPredicateListDelegate checkedAction = null , ModifyPredicateListDelegate uncheckedAction = null) {
            //Style style = Application.Current.FindResource("ToggleButtonStyle") as Style;
            //var result = new ToggleButton() {
            //    ToolTip = cell.ToString() ,
            //    Style = style
            //};
            var result = new MaybeUnoccupiedToggleButton() {
                ToolTip = cell.ToString()
            };
            if (checkedAction != null) result.Checked += (sender , args) => { checkedAction(cell.ConstraintPredicate); };
            if (uncheckedAction != null) result.Unchecked += (sender , args) => { uncheckedAction(cell.ConstraintPredicate); };
            return result;
        }


        public UIElement CreateDefinitelyUnoccupiedState() {
            return new Border() { Background = Brushes.DarkGray , Height = 50 };
        }
    }

    public class RealStateElementFactory : MockStateElementFactory {
        private ModifyPredicateListDelegate _checkAction;
        private ModifyPredicateListDelegate _uncheckedAction;
        public RealStateElementFactory(ModifyPredicateListDelegate checkAction , ModifyPredicateListDelegate uncheckedAction) {
            this._checkAction = checkAction;
            this._uncheckedAction = uncheckedAction;
        }


        public override UIElement CreateMaybeUnoccupiedState(IStateCell cell , ModifyPredicateListDelegate checkedAction = null ,
                                                             ModifyPredicateListDelegate uncheckedAction = null) {
            return base.CreateMaybeUnoccupiedState(cell , _checkAction , _uncheckedAction);
        }
    }
}
