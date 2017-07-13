using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Class.StateSummary;
using Time_Table_Arranging_Program.Pages;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program {
    /// <summary>
    ///     Interaction logic for TimeTableGUI.xaml
    /// </summary>
    public partial class TimeTableGUI : UserControl {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimetableProperty =
            DependencyProperty.Register("Timetable" , typeof(ITimetable) , typeof(TimeTableGUI) ,
                new PropertyMetadata(null , OnInputChanged));

        private readonly List<UIElement> _addedElement = new List<UIElement>();
        private readonly int _eachDayHaveHowManyRow = 5;
        private readonly int _maxTime = Global.MaxTime;
        private readonly int _minTime = 7;
        private readonly int _paddingDueToTimeRow = 2;
        private readonly int _paddingDueToDayColumn = 1;
        private Dictionary<int , HashSet<int>> _occupiedIndex;


        public TimeTableGUI() {
            InitializeComponent();
            NumberOfColumns = (_maxTime - _minTime) * 2 + 1;
            NumberOfRows = 7 * _eachDayHaveHowManyRow + 2;
            BuildBorders();
            BuildTimeRow();
            BuildDayColumn();
            _occupiedIndex = new Dictionary<int , HashSet<int>>();
        }

        public ITimetable Timetable {
            get { return (ITimetable)GetValue(TimetableProperty); }
            set { SetValue(TimetableProperty , value); }
        }

        private int NumberOfRows {
            get { return Grid.RowDefinitions.Count; }
            set {
                Grid.RowDefinitions.Clear();
                for (var i = 0 ; i < value ; i++)
                    Grid.RowDefinitions.Add(new RowDefinition());
            }
        }

        private int NumberOfColumns {
            get { return Grid.ColumnDefinitions.Count; }
            set {
                Grid.ColumnDefinitions.Clear();
                for (var i = 0 ; i < value ; i++)
                    Grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(35) });
            }
        }

        private static void OnInputChanged(DependencyObject d , DependencyPropertyChangedEventArgs e) {
            var timetableGUI = d as TimeTableGUI;
            timetableGUI.ClearGui();
            timetableGUI.GenerateGui((ITimetable)e.NewValue);
        }

        private void BuildDayColumn() {
            Grid.ColumnDefinitions[0].Width = new GridLength(100);
            string[] days = { "MON" , "TUE" , "WED" , "THU" , "FRI" , "SAT" , "SUN" };
            var rowIndex = 2;
            for (var i = 0 ; i < days.Length ; i++) {
                var lbl = new Label {
                    Content = days[i] ,
                    HorizontalAlignment = HorizontalAlignment.Center ,
                    FontWeight = FontWeights.Bold ,
                    FontFamily = new FontFamily("Consolas") ,
                    FontSize = 15 ,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.Children.Add(lbl);
                Grid.SetColumn(lbl , 0);
                Grid.SetRow(lbl , rowIndex);
                Grid.SetRowSpan(lbl , _eachDayHaveHowManyRow);
                rowIndex += _eachDayHaveHowManyRow;
            }
        }

        private void BuildBorders() {
            //building horizontal borders
            //for (var i = 0 + 2; i < 2 + 7*_eachDayHaveHowManyRow; i++) {
            //    var border = GetBorder();
            //    Grid.Children.Add(border);
            //    Grid.SetRow(border, i);
            //    if (i > 3) Grid.SetRowSpan(border, _eachDayHaveHowManyRow);
            //    Grid.SetColumn(border, 0);
            //    Grid.SetColumnSpan(border, NumberOfColumns);
            //}

            //building vertical borders
            for (var i = 0 ; i < NumberOfColumns ; i++) {
                var border = GetBorder();
                Grid.Children.Add(border);
                Grid.SetColumn(border , i);
                Grid.SetRow(border , 0);
                Grid.SetRowSpan(border , 2 + 7 * _eachDayHaveHowManyRow);
            }

            //building horizontal day borders
            for (var i = 0 + 2 ; i < 2 + 7 * _eachDayHaveHowManyRow ; i += _eachDayHaveHowManyRow) {
                var border = new Border { BorderThickness = new Thickness(0.5) , BorderBrush = Brushes.Black };
                Grid.Children.Add(border);
                Grid.SetColumn(border , 0);
                Grid.SetColumnSpan(border , NumberOfColumns);
                Grid.SetRow(border , i);
                Grid.SetRowSpan(border , _eachDayHaveHowManyRow);
            }
        }

        private Border GetBorder() {
            return new Border {
                BorderThickness = new Thickness(0.1) ,
                BorderBrush = Brushes.Gray ,
                Background = Brushes.WhiteSmoke
            };
        }

        private void BuildTimeRow() {
            var columnIndex = 1;
            for (var i = _minTime ; i < _maxTime ; i++) {
                var label1 = GetLabel();
                label1.Content = $"{(i > 12 ? i - 12 : i)}:00";
                Grid.Children.Add(label1);
                Grid.SetColumn(label1 , columnIndex);
                Grid.SetRow(label1 , 0);
                Grid.SetColumnSpan(label1 , 2);
                var label2 = GetLabel();
                label2.Content = $"{(i > 12 ? i - 12 : i) + 1}:00";
                Grid.Children.Add(label2);
                Grid.SetColumn(label2 , columnIndex);
                Grid.SetRow(label2 , 1);
                Grid.SetColumnSpan(label2 , 2);
                columnIndex += 2;
            }
        }

        private Label GetLabel() {
            return new Label {
                BorderThickness = new Thickness(1) ,
                BorderBrush = Brushes.Black ,
                HorizontalContentAlignment = HorizontalAlignment.Center ,
                FontWeight = FontWeights.Bold ,
            };
        }

        private int GetColumnSpan(TimeSpan time) {
            double result = time.TotalHours * 2;
            return (int)result;
        }

        private int GetRowIndex(Day day) {
            return (day.IntValue - 1) * _eachDayHaveHowManyRow + 2;
        }

        private int GetColumnIndex(ITime startTime) {
            var result = 0;
            result = (startTime.Hour - _minTime) * 2 + 1;
            if (startTime.Minute == 30) result++;
            return result;
        }


        public void ClearGui() {
            NoOfSelectedSubjectLabel.Content = "";
            for (var i = 0 ; i < _addedElement.Count ; i++) {
                var ui = _addedElement[i];
                if (Grid.Children.Contains(ui))
                    Grid.Children.Remove(ui);
            }
            _addedElement.Clear();
            DescriptionViewer.Clear();
            _occupiedIndex = new Dictionary<int , HashSet<int>>();
        }

        public void GenerateGui(ITimetable timetable, bool getPartialInfoOnly = true) {
            var slots = timetable.ToList();
            ClearGui();
            slots.Sort();
            NoOfSelectedSubjectLabel.Content = GetSubjectCount(slots);
            GenerateTimetableView(slots, getPartialInfoOnly);
            DescriptionViewer.Update(slots);
        }

        private int GetSubjectCount(List<Slot> slots) {
            slots.Sort();
            var count = 1;
            var previousSubjectName = slots[0].SubjectName;
            foreach (var s in slots) {
                if (s.SubjectName != previousSubjectName) count++;
                previousSubjectName = s.SubjectName;
            }
            return count;
        }

        private void GenerateTimetableView(List<Slot> slots, bool getPartialInfoOnly = true) {
            var previousSubjectName = slots[0].SubjectName;
            IColorGenerator c = new ColorGenerator();
            foreach (var s in slots) {
                if (s.SubjectName != previousSubjectName) c.GoToNextColor();
                var box = GenerateBox(s , c, getPartialInfoOnly);
                var colIndex = GetColumnIndex(s.StartTime);
                var rowIndex = GetRowIndex(s.Day);
                var columnSpan = GetColumnSpan(s.EndTime.Minus(s.StartTime));
                while (IsOccupied(rowIndex , colIndex , columnSpan)) {
                    rowIndex++;
                }
                AddToGrid(box , colIndex , rowIndex , columnSpan);

                UpdateOccupiedIndex(rowIndex , colIndex , columnSpan);
                previousSubjectName = s.SubjectName;
            }
        }

        private void AddToGrid(UIElement elem , int colIndex , int rowIndex , int columnSpan) {
            Grid.Children.Add(elem);
            Grid.SetColumn(elem , colIndex);
            Grid.SetRow(elem , rowIndex);
            Grid.SetColumnSpan(elem , columnSpan);
            _addedElement.Add(elem);

        }

        private void UpdateOccupiedIndex(int rowIndex , int colIndex , int columnSpan) {
            if (!_occupiedIndex.ContainsKey(rowIndex))
                _occupiedIndex.Add(rowIndex , new HashSet<int>());
            for (int i = 0 ; i < columnSpan ; i++) {
                _occupiedIndex[rowIndex].Add(colIndex + i);
            }
        }

        private bool IsOccupied(int rowIndex , int colIndex , int columnSpan) {
            if (!_occupiedIndex.ContainsKey(rowIndex)) return false;
            for (int i = 0 ; i < columnSpan ; i++) {
                if (_occupiedIndex[rowIndex].Contains(colIndex + i))
                    return true;
            }
            return false;
        }


        private Border GenerateBox(Slot s , IColorGenerator c, bool getPartialInfoOnly = true) {
            var textblock = new TextBlock {                
                Margin = new Thickness(2) ,
                TextAlignment = TextAlignment.Center ,
                FontWeight = FontWeights.DemiBold ,
                VerticalAlignment = VerticalAlignment.Center
            };
            textblock.Text = getPartialInfoOnly ? GetPartialInfo(s) : GetFullInfo(s);
            var border = new Border {
                BorderThickness = new Thickness(0.75) ,
                BorderBrush = Brushes.Black ,
                Child = textblock ,
                Background = c.GetCurrentBrush() ,
                CornerRadius = new CornerRadius(2) ,
                Height = 50 ,
                ForceCursor = true ,
                ToolTip = GetTooltip(s) , // s.SubjectName ,                 
            };
            var originalColor = c.GetCurrentColor();
            var originalBrush = c.GetCurrentBrush();
            border.MouseEnter += (sender , args) => {
                border.Background = new SolidColorBrush(originalColor.Darker());
            };
            border.MouseLeave += (sender , args) => {
                border.Background = originalBrush;
            };
            return border;
        }

        private string GetFullInfo(Slot s) {
            return $"{s.SubjectName.GetInitial()} ({s.Type}{s.Number})\n{s.Venue}\n{s.WeekNumber}";
        }

        private string GetTooltip(Slot s) {
            return $"{s.SubjectName}\n{s.Code}";
        }

        private string GetPartialInfo(Slot s) {
            string result = "";
            result += $" {s.SubjectName.GetInitial()} ({s.Type}{s.Number})";
            if (Windows_Settings.SearchByConsideringWeekNumber.IsChecked ||
                !s.Number.Contains("/"))
                result += $"\n{s.WeekNumber}\n{s.Venue}";
            else
                result += "\n-\n-";            
            return result;
        }


        public void GenerateStateSummary(List<List<Slot>> outputTimetables , IStateElementFactory factory) {
            ClearGui();
            var bg = CustomBackgroundWorker<List<List<Slot>> , StateTable>.
    RunAndShowLoadingScreen(StateTable.Parse , outputTimetables , "Calculating state table . . .");
            var stateTable = bg.GetResult();
            foreach (var cell in stateTable) {
                int paddedRowIndex = cell.RowIndex * _eachDayHaveHowManyRow + _paddingDueToTimeRow;
                int paddedColIndex = cell.ColumnIndex + _paddingDueToDayColumn;
                int colSpan = StateCell.ColumnSpanOfEachCell;
                switch (cell.State) {
                    case CellState.DefinitelyOccupied:
                        AddToGrid(factory.CreateDefinitelyOccupiedState() , paddedColIndex , paddedRowIndex , colSpan);
                        break;
                    case CellState.MaybeUnoccupied:
                        AddToGrid(factory.CreateMaybeUnoccupiedState(cell) , paddedColIndex , paddedRowIndex , colSpan);
                        break;
                    case CellState.DefinitelyUnoccupied:
                        AddToGrid(factory.CreateDefinitelyUnoccupiedState() , paddedColIndex , paddedRowIndex , 1);
                        break;
                }
            }
        }

        public void RegenerateStateSummary(List<List<Slot>> outputTimetables , IStateElementFactory factory) {
            var bg = CustomBackgroundWorker<List<List<Slot>> , StateTable>.
RunAndShowLoadingScreen(StateTable.Parse , outputTimetables , "Recalculating . . .");
            var stateTable = bg.GetResult();
            foreach (var cell in stateTable) {
                int paddedRowIndex = cell.RowIndex * _eachDayHaveHowManyRow + _paddingDueToTimeRow;
                int paddedColIndex = cell.ColumnIndex + _paddingDueToDayColumn;
                int colSpan = StateCell.ColumnSpanOfEachCell;
                var element =
                    Grid.Children
                        .Cast<UIElement>()
                        .First(e =>
                            Grid.GetRow(e) == paddedRowIndex &&
                            Grid.GetColumn(e) == paddedColIndex);
                if (element is ToggleButton) {
                    if (((ToggleButton)element).IsChecked.Value) continue;
                }

                switch (cell.State) {
                    case CellState.DefinitelyOccupied:
                        if (element != null) Grid.Children.Remove(element);
                        AddToGrid(factory.CreateDefinitelyOccupiedState() , paddedColIndex , paddedRowIndex , colSpan);
                        break;
                    case CellState.MaybeUnoccupied:
                        if (element != null) Grid.Children.Remove(element);
                        AddToGrid(factory.CreateMaybeUnoccupiedState(cell) , paddedColIndex , paddedRowIndex , colSpan);
                        break;
                    case CellState.DefinitelyUnoccupied:
                        if (element != null) Grid.Children.Remove(element);
                        AddToGrid(factory.CreateDefinitelyUnoccupiedState() , paddedColIndex , paddedRowIndex , 1);
                        break;
                }
            }
        }
    }
}