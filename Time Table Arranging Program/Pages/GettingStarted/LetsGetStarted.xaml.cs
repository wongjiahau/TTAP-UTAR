using System.Windows;
using System.Windows.Controls;

namespace Time_Table_Arranging_Program.Pages.GettingStarted {
    /// <summary>
    /// Interaction logic for LetsGetStarted.xaml
    /// </summary>
    public partial class LetsGetStarted : UserControl {
        public LetsGetStarted() {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            Global.MainFrame.Navigate(new Page_Login());
        }
    }
}