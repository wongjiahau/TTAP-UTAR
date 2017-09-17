using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Time_Table_Arranging_Program.User_Control.CheckboxWithListDownMenuFolder.ErrorMessageType {
    /// <summary>
    /// Interaction logic for TypeTwoError.xaml
    /// </summary>
    public partial class GroupClashingError : UserControl {
        public GroupClashingError() {
            InitializeComponent();
            ErrorTextBlock.AddHandler(Hyperlink.RequestNavigateEvent,
                new RequestNavigateEventHandler(Hyperlink_RequestNavigate));
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e) {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}