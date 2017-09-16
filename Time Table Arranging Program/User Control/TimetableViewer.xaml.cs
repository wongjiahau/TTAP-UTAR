using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.MVVM_Framework.Models;
using Time_Table_Arranging_Program.MVVM_Framework.ViewModels;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    /// Interaction logic for TimetableViewer.xaml
    /// </summary>
    public partial class TimetableViewer : UserControl, IPureObserver<ITimetableList> {
        private CyclicIndex _cyclicIndex;
        private IImmutableObservable<ITimetableList> _observableTimetableList;
        private List<ITimetable> _outputTimeTable = new List<ITimetable>();

        public TimetableViewer() {
            InitializeComponent();
            MessageToBeDisplayedWhenOutputTimetableIsEmpty = "No possible combination :( ";
        }

        public string MessageToBeDisplayedWhenOutputTimetableIsEmpty { get; set; }

        public void SetObservedThings(IImmutableObservable<ITimetableList> x) {
            _observableTimetableList = x;
            _observableTimetableList.RegisterObserver(this);
        }

        public void Update() {
            ITimetableList currentState = _observableTimetableList.GetCurrentState();
            if (currentState.IsEmpty()) {
                InstructionLabel.Content = currentState.Message;
                TtapIcon.Visibility = Visibility.Visible;
                //InstructionLabel.Visibility = Visibility.Visible;
                TimeTableGui.Visibility = Visibility.Collapsed;
                TimeTableGui.ClearGui();
                return;
            }
            _outputTimeTable = currentState.ToList();
            TimeTableGui.GenerateGui(_outputTimeTable[0]);
            TtapIcon.Visibility = Visibility.Collapsed;
            //InstructionLabel.Visibility = Visibility.Collapsed;
            TimeTableGui.Visibility = Visibility.Visible;
            //  IndexViewer.Initialize(_cyclicIndex);
        }

        public void Initialize(CyclicIndex cyclicIndex) {
            _cyclicIndex = cyclicIndex;
            _cyclicIndex.CurrentValueChanged += CyclicIndexOnCurrentValueChanged;
            CyclicIndexOnCurrentValueChanged(null, null);
        }

        private void CyclicIndexOnCurrentValueChanged(object sender, EventArgs eventArgs) {
            if (_cyclicIndex.CurrentValue < 0) return;
            TimeTableGui.GenerateGui(_outputTimeTable[_cyclicIndex.CurrentValue]);
            ViewChanged(this, null);
        }

        public event EventHandler ViewChanged;

        public bool JustBuilded() {
            return _cyclicIndex.CurrentValue == 0;
        }

        public void Update(List<ITimetable> outputTimeTable, bool inputSlotsIsEmpty) {
            if (outputTimeTable == null || outputTimeTable.Count == 0) {
                if (inputSlotsIsEmpty)
                    InstructionLabel.Content = "Please select your subjects";
                else
                    InstructionLabel.Content = MessageToBeDisplayedWhenOutputTimetableIsEmpty;
                InstructionLabel.Visibility = Visibility.Visible;
                TimeTableGui.Visibility = Visibility.Collapsed;
                TimeTableGui.ClearGui();

                return;
            }
            _outputTimeTable = outputTimeTable;
            TimeTableGui.GenerateGui(_outputTimeTable[0]);
            InstructionLabel.Visibility = Visibility.Collapsed;
            TimeTableGui.Visibility = Visibility.Visible;
            _cyclicIndex.MaxValue = _outputTimeTable.Count - 1;
        }

        public ITimetable GetCurrentTimetable() {
            return _outputTimeTable[_cyclicIndex.CurrentValue];
        }
    }
}