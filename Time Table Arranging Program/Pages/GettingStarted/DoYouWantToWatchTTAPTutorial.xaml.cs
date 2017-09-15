using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Time_Table_Arranging_Program.Pages.Login;

namespace Time_Table_Arranging_Program.Pages.GettingStarted {
    /// <summary>
    /// Interaction logic for DoYouWantToWatchTTAPTutorial.xaml
    /// </summary>
    public partial class DoYouWantToWatchTTAPTutorial : UserControl {
        public DoYouWantToWatchTTAPTutorial() {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            Process.Start(new ProcessStartInfo(new UrlProvider().ReadMeUrl));
        }
    }
}