using System;
using System.Collections.Generic;
using System.Windows;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.User_Control.SubjectViewFolder;

namespace Time_Table_Arranging_Program.Windows_Control {
    public partial class Window_ChooseSpecificSlot : Window {
        private readonly ChooseSpecificSlotModel _model;
        public Window_ChooseSpecificSlot(ChooseSpecificSlotModel model) {
            InitializeComponent();
            _model = model;
            this.DataContext = _model;
            InitializeUi(_model.SelectedSubjects);
        }

        private void InitializeUi(List<SubjectModel> selectedSubjects) {
            foreach (var subject in selectedSubjects) {
                var s = new SubjectViewForChoosingSlots();
                s.SetDataContext(subject);
                s.SlotSelectionChanged += SlotSelectionChanged;
                s.ListOfSlotSelected += ListOfSlotSelected;
                s.ListOfSlotDeselected += ListOfSlotDeselected;
                StackPanel.Children.Add(s);
            }
        }

        private void ListOfSlotSelected(object sender, EventArgs eventArgs) {
            _model.SelectSlots((List<int>)sender);
        }

        private void ListOfSlotDeselected(object sender, EventArgs eventArgs) {
            _model.DeselectSlots((List<int>)sender);
        }
        
        private void SlotSelectionChanged(object sender , EventArgs e) {
            InfoStackPanel.Visibility = Visibility.Visible;
            var targetSlot = (Slot)sender;
            if (targetSlot.IsSelected) _model.SelectSlot(targetSlot.UID);
            else _model.DeselectSlot(targetSlot.UID);
        }

        public List<List<Slot>> NewListOfTimetables { get; private set; } = null;

        private void BackButton_OnClick(object sender, RoutedEventArgs e) {
            UserClickedDone = false;
            Hide();
        }

        private void DoneButton_OnClick(object sender, RoutedEventArgs e) {
            _model.CheckForError();
            if (_model.GotError) return;
            UserClickedDone = true;
            Hide();
        }

        public bool UserClickedDone { get; private set; }

        public bool SlotSelectionIsChanged => _model.SlotSelectionIsChanged;
    }
}
