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

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    /// Interaction logic for TimetableGUI_Full.xaml
    /// </summary>
    public partial class TimetableGUI_Full : UserControl {
        public TimetableGUI_Full() {
            InitializeComponent();
        }

        public void GenerateGUI(ITimetable timetable) {
            this.TimeTableGui.GenerateGui(timetable,false);
            this.TimetableDescriptionViewer.GenerateAsImage(timetable.ToList());
        }
    }
}
