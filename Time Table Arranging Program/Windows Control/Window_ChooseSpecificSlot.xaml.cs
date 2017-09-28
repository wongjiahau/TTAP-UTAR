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
            InitializeUi(_model.Subjects);
        }

        private void InitializeUi(List<SubjectModel> selectedSubjects) {
            foreach (var subject in selectedSubjects) {
                var s = new SubjectViewForChoosingSlots();
                s.SetDataContext(subject);
                StackPanel.Children.Add(s);
            }
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

        public bool SlotSelectionIsChanged => _model.IsSlotSelectionChanged;
    }
}
