using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program {
    /// <summary>
    ///     Interaction logic for TimeTableGUI.xaml
    /// </summary>
    public partial class TimeTableGUI : UserControl {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimetableProperty =
            DependencyProperty.Register("Timetable", typeof(ITimetable), typeof(TimeTableGUI),
                new PropertyMetadata(null, OnInputChanged));

        private readonly List<UIElement> _addedSlot = new List<UIElement>();
        private readonly int _eachDayHaveHowManyRow = 5;
        private readonly int _maxTime = Global.MaxTime;
        private readonly int minTime = 7;
        private Dictionary<int, HashSet<int>> _occupiedIndex;


        public TimeTableGUI() {
            InitializeComponent();
            NumberOfColumns = (_maxTime - minTime)*2 + 1;
            NumberOfRows = 7*_eachDayHaveHowManyRow + 2;
            BuildBorders();
            BuildTimeRow();
            BuildDayColumn();
            _occupiedIndex = new Dictionary<int, HashSet<int>>();
        }

        public ITimetable Timetable {
            get { return (ITimetable) GetValue(TimetableProperty); }
            set { SetValue(TimetableProperty, value); }
        }

        private int NumberOfRows {
            get { return Grid.RowDefinitions.Count; }
            set
            {
                Grid.RowDefinitions.Clear();
                for (var i = 0; i < value; i++)
                    Grid.RowDefinitions.Add(new RowDefinition());
            }
        }

        private int NumberOfColumns {
            get { return Grid.ColumnDefinitions.Count; }
            set
            {
                Grid.ColumnDefinitions.Clear();
                for (var i = 0; i < value; i++)
                    Grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(35)});
            }
        }

        private static void OnInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var timetableGUI = d as TimeTableGUI;
            timetableGUI.ClearGui();
            timetableGUI.GenerateGui((ITimetable) e.NewValue);
        }

        private void BuildDayColumn() {
            Grid.ColumnDefinitions[0].Width = new GridLength(100);
            string[] days = {"MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN"};
            var rowIndex = 2;
            for (var i = 0; i < days.Length; i++) {
                var lbl = new Label
                {
                    Content = days[i],
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold,
                    FontFamily = new FontFamily("Consolas"),
                    FontSize = 15,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.Children.Add(lbl);
                Grid.SetColumn(lbl, 0);
                Grid.SetRow(lbl, rowIndex);
                Grid.SetRowSpan(lbl, _eachDayHaveHowManyRow);
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
            for (var i = 0; i < NumberOfColumns; i++) {
                var border = GetBorder();
                Grid.Children.Add(border);
                Grid.SetColumn(border, i);
                Grid.SetRow(border, 0);
                Grid.SetRowSpan(border, 2 + 7*_eachDayHaveHowManyRow);
            }

            //building horizontal day borders
            for (var i = 0 + 2; i < 2 + 7*_eachDayHaveHowManyRow; i += _eachDayHaveHowManyRow) {
                var border = new Border {BorderThickness = new Thickness(0.5), BorderBrush = Brushes.Black};
                Grid.Children.Add(border);
                Grid.SetColumn(border, 0);
                Grid.SetColumnSpan(border, NumberOfColumns);
                Grid.SetRow(border, i);
                Grid.SetRowSpan(border, _eachDayHaveHowManyRow);
            }
        }

        private Border GetBorder() {
            return new Border
            {
                BorderThickness = new Thickness(0.1),
                BorderBrush = Brushes.Gray,
                Background = Brushes.WhiteSmoke
            };
        }

        private void BuildTimeRow() {
            var columnIndex = 1;
            for (var i = minTime; i < _maxTime; i++) {
                var label1 = GetLabel();
                label1.Content = $"{(i > 12 ? i - 12 : i)}:00";
                Grid.Children.Add(label1);
                Grid.SetColumn(label1, columnIndex);
                Grid.SetRow(label1, 0);
                Grid.SetColumnSpan(label1, 2);
                var label2 = GetLabel();
                label2.Content = $"{(i > 12 ? i - 12 : i) + 1}:00";
                Grid.Children.Add(label2);
                Grid.SetColumn(label2, columnIndex);
                Grid.SetRow(label2, 1);
                Grid.SetColumnSpan(label2, 2);
                columnIndex += 2;
            }
        }

        private Label GetLabel() {
            return new Label
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Black,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeights.Bold
            };
        }

        private int GetColumnSpan(TimeSpan time) {
            double result = time.TotalHours*2;
            return (int) result;
        }

        private int GetRowIndex(string day) {
            string[] days = {"Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"};
            return days.ToList().FindIndex(x => x == day)*_eachDayHaveHowManyRow + 2;
        }

        private int GetColumnIndex(ITime startTime) {
            var result = 0;
            result = (startTime.Hour - minTime)*2 + 1;
            if (startTime.Minute == 30) result++;
            return result;
        }


        public void ClearGui() {
            NoOfSelectedSubjectLabel.Content = "";
            for (var i = 0; i < _addedSlot.Count; i++) {
                var ui = _addedSlot[i];
                if (Grid.Children.Contains(ui))
                    Grid.Children.Remove(ui);
            }
            _addedSlot.Clear();
            DescriptionViewer.Clear();
            _occupiedIndex = new Dictionary<int, HashSet<int>>();
        }

        public void GenerateGui(ITimetable timetable) {
            var slots = timetable.ToList();
            ClearGui();
            slots.Sort();
            NoOfSelectedSubjectLabel.Content = GetSubjectCount(slots);
            GenerateTimetableView(slots);
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


        private void GenerateTimetableView(List<Slot> slots) {
            var previousSubjectName = slots[0].SubjectName;
            IColorGenerator c = new ColorGenerator();
            foreach (var s in slots) {
                if (s.SubjectName != previousSubjectName) c.GoToNextColor();
                var box = GenerateBox(s, c);

                Grid.Children.Add(box);
                var colIndex = GetColumnIndex(s.StartTime);
                var rowIndex = GetRowIndex(s.Day.StringValue);
                var columnSpan = GetColumnSpan(s.EndTime.Minus(s.StartTime));
                while (IsOccupied(rowIndex, colIndex, columnSpan)) {
                    rowIndex++;
                }

                Grid.SetColumn(box, colIndex);
                Grid.SetRow(box, rowIndex);
                Grid.SetColumnSpan(box, columnSpan);
                _addedSlot.Add(box);
                UpdateOccupiedIndex(rowIndex, colIndex, columnSpan);
                previousSubjectName = s.SubjectName;
            }
        }

        private void UpdateOccupiedIndex(int rowIndex, int colIndex, int columnSpan) {
            if (!_occupiedIndex.ContainsKey(rowIndex))
                _occupiedIndex.Add(rowIndex, new HashSet<int>());
            for (int i = 0; i < columnSpan; i++) {
                _occupiedIndex[rowIndex].Add(colIndex + i);
            }
        }

        private bool IsOccupied(int rowIndex, int colIndex, int columnSpan) {
            if (!_occupiedIndex.ContainsKey(rowIndex)) return false;
            for (int i = 0; i < columnSpan; i++) {
                if (_occupiedIndex[rowIndex].Contains(colIndex + i))
                    return true;
            }
            return false;


            var occupiedColumn = new HashSet<int>();
            foreach (var ui in Grid.Children.Cast<UIElement>()) {
                if (Grid.GetRow(ui) == rowIndex && ui.GetType() == typeof(Border)) {
                    if ((ui as Border).Child == null) continue;
                    for (var i = 0; i < Grid.GetColumnSpan(ui); i++) {
                        occupiedColumn.Add(Grid.GetColumn(ui) + i);
                    }
                }
            }
            return occupiedColumn.Contains(colIndex);
        }


        private Border GenerateBox(Slot s, IColorGenerator c) {
            var textblock = new TextBlock
            {
                Text = GetPartialInfo(s),
                Margin = new Thickness(2),
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.DemiBold,
                VerticalAlignment = VerticalAlignment.Center
            };
            var border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Black,
                Child = textblock,
                Background = c.GetCurrentBrush(),
                Height = 50,
                ToolTip = s.SubjectName,
                Cursor = Cursors.Help
            };
            border.MouseEnter += (sender, args) => { border.BorderThickness = new Thickness(4); };
            border.MouseLeave += (sender, args) => { border.BorderThickness = new Thickness(1); };
            return border;
        }


        private string GetPartialInfo(Slot s) {
            return $" {s.SubjectName.GetInitial()}({s.Type}{s.Number})\n{s.Venue}\n{s.WeekNumber}";
        }
    }
}