using System.Windows.Controls;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    /// Interaction logic for TimetableGUI_Full.xaml
    /// </summary>
    public partial class TimetableGUI_Full : UserControl {
        public TimetableGUI_Full() {
            InitializeComponent();
        }

        public void GenerateGUI(ITimetable timetable) {
            TimeTableGui.GenerateGui(timetable);
            TimetableDescriptionViewer.GenerateAsImage(timetable.ToList());
        }
    }
}