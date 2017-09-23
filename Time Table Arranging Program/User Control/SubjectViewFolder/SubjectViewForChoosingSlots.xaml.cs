using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Model;

namespace Time_Table_Arranging_Program.User_Control.SubjectViewFolder {
    /// <summary>
    /// Interaction logic for SubjectViewForChoosingSlots.xaml
    /// </summary>
    public partial class SubjectViewForChoosingSlots : UserControl {
        public event EventHandler SlotSelectionChanged;
        public SubjectViewForChoosingSlots() {
            InitializeComponent();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender , MouseButtonEventArgs e) {
            var item = sender as ListViewItem;
            var slot = item?.Content as Slot;
            if (slot == null) return;
            slot.IsSelected = !slot.IsSelected;
            CascadeEffectToSimilarSlot(slot);
            SlotSelectionChanged?.Invoke(slot, null);
        }

        private void CascadeEffectToSimilarSlot(Slot selectedSlot) {
            foreach (Slot s in ListView.Items) {
                if (s.UID != selectedSlot.UID) continue;
                s.IsSelected = selectedSlot.IsSelected;
            }
            UpdateListView();
        }

        private void UpdateListView() {
            var temp = ListView.ItemsSource;
            ListView.ItemsSource = null;
            ListView.ItemsSource = temp;
        }

        private void ToggleCheckButton_OnClick(object sender , RoutedEventArgs e) {
            if (ToggleCheckButton.Content.ToString() == "Untick all slots") {
                ToggleCheckButton.Content = "Tick all slots";
                InstructionLabel.Content = ". . . Tick the slots that you want";
                foreach (var item in ListView.ItemsSource) {
                    ((Slot)item).IsSelected = false;
                }
            }
            else {
                ToggleCheckButton.Content = "Untick all slots";
                InstructionLabel.Content = ". . . Untick the slots that you don't want";
                foreach (var item in ListView.ItemsSource) {
                    ((Slot)item).IsSelected = true;
                }
            }
            UpdateListView();
        }
    }
}
