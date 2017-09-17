using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;
using Time_Table_Arranging_Program.Pages.Login;

namespace Time_Table_Arranging_Program.Pages {
    /// <summary>
    /// Interaction logic for WelcomeToTTAP.xaml
    /// </summary>
    public partial class WelcomeToTTAP : UserControl {
        public WelcomeToTTAP() {
            InitializeComponent();
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e) {
            Process.Start(new ProcessStartInfo(new UrlProvider().DownloadLink));
            e.Handled = true;
        }
    }
}