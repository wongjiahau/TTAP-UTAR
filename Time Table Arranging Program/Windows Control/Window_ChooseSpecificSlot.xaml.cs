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
using System.Windows.Shapes;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.User_Control.SubjectViewFolder;

namespace Time_Table_Arranging_Program.Windows_Control {
    /// <summary>
    /// Interaction logic for Window_ChooseSpecificSlot.xaml
    /// </summary>
    public partial class Window_ChooseSpecificSlot : Window {
        private readonly List<List<Slot>> _originalTimetables;
        private readonly List<SubjectModel> _selectedSubjects;
        public Window_ChooseSpecificSlot(List<List<Slot>> originalTimetables , List<SubjectModel> selectedSubjects) {
            _originalTimetables = originalTimetables;
            _selectedSubjects = selectedSubjects;
            InitializeComponent();
            InitializeUi();
        }

        private void InitializeUi() {
            foreach (var subject in _selectedSubjects) {
                var s = new SubjectViewForChoosingSlots() { DataContext = subject };
                s.SlotSelectionChanged += SlotSelectionChanged;
                StackPanel.Children.Add(s);
            }
        }

        private void SlotSelectionChanged(object sender , EventArgs e) {

        }

        public List<List<Slot>> NewListOfTimetables { get; private set; } = null;
    }
}
