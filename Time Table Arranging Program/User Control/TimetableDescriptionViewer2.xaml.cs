using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace Time_Table_Arranging_Program.User_Control {
    public class SubjectSummary {
        public string SubjectName { get; set; }
        public string Code { get; set; }
        public string TypesAndNumber { get; set; } //e.g. "L1, T2, P4"        
        public Brush Color { get; set; }
    }

    public class SubjectSummaries : List<SubjectSummary> { }


    /// <summary>
    /// Interaction logic for TimetableDescriptionViewer2.xaml
    /// </summary>
    public partial class TimetableDescriptionViewer2 : UserControl {
        public static ObservableCollection<SubjectSummary> SubjectDescriptions = new ObservableCollection
            <SubjectSummary>
            {
                new SubjectSummary
                {
                    SubjectName = "English for Professionals",
                    Code = "MPU3113",
                    Color = Brushes.Aqua,
                    TypesAndNumber = "L1, T2, P4"
                },
                new SubjectSummary
                {
                    SubjectName = "Advance Database System",
                    Code = "UESS6533",
                    Color = Brushes.PaleVioletRed,
                    TypesAndNumber = "L1, T2"
                }
            };

        public TimetableDescriptionViewer2() {
            InitializeComponent();
        }
    }
}