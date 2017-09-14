using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Time_Table_Arranging_Program.Pages.Login;

namespace Time_Table_Arranging_Program.Pages
{
    /// <summary>
    /// Interaction logic for WelcomeToTTAP.xaml
    /// </summary>
    public partial class WelcomeToTTAP : UserControl
    {
        public WelcomeToTTAP()
        {
            InitializeComponent();
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e) {
            Process.Start(new ProcessStartInfo(new UrlProvider().DownloadLink));
            e.Handled = true;
        }
    }
}
