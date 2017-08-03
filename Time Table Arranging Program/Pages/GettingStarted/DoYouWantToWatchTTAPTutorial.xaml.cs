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

namespace Time_Table_Arranging_Program.Pages.GettingStarted {
    /// <summary>
    /// Interaction logic for DoYouWantToWatchTTAPTutorial.xaml
    /// </summary>
    public partial class DoYouWantToWatchTTAPTutorial : UserControl {
        public DoYouWantToWatchTTAPTutorial() {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            Process.Start(new ProcessStartInfo(MainWindow.HelpGifUrl));
        }
    }
}
