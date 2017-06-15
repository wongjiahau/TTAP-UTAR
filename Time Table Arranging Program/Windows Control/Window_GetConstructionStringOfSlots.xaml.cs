using System.Collections.Generic;
using System.Windows;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.Windows_Control {
    /// <summary>
    ///     Interaction logic for Window_GetConstructionStringOfSlots.xaml
    /// </summary>
    public partial class Window_GetConstructionStringOfSlots : Window {
        public Window_GetConstructionStringOfSlots(List<Slot> input) {
            InitializeComponent();
            foreach (var s in input) {
                TextBox.Text += s.ToConstructionString() + ",\n";
            }
        }
    }
}