using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;

namespace Time_Table_Arranging_Program.User_Control.SubjectViewFolder {
    public partial class SubjectViewForChoosingSlots : UserControl, INeedDataContext<SubjectModel> {
        private SubjectModel _model;
        public event EventHandler SlotSelectionChanged;
        public event EventHandler ListOfSlotSelected;
        public event EventHandler ListOfSlotDeselected;
        public SubjectViewForChoosingSlots() => InitializeComponent();

        public void SetDataContext(SubjectModel subjectModel) {
            _model = subjectModel;
            this.DataContext = _model;
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender , MouseButtonEventArgs e) {
            var item = sender as ListViewItem;
            var slot = item?.Content as Slot;
            if (slot == null) return;
            _model.ToggleSelectionOn(slot);
            UpdateListView();
        }

        private void ToggleCheckButton_OnClick(object sender , RoutedEventArgs e) {
            UpdateListView();
        }

        private void UpdateListView() {
            var temp = ListView.ItemsSource;
            ListView.ItemsSource = null;
            ListView.ItemsSource = temp;
        }

    }
}
