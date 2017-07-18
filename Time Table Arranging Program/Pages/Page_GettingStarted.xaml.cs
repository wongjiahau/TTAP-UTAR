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

namespace Time_Table_Arranging_Program.Pages {
    /// <summary>
    /// Interaction logic for Page_GettingStarted.xaml
    /// </summary>
    public partial class Page_GettingStarted : Page {
        public Page_GettingStarted() {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
        }

        private void GotItButton_Click(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(new Page_Intro());
        }

        private void Page_OnLoaded(object sender, RoutedEventArgs e) {            
            var result = MessageBox.Show("Do you want to watch tutorial of TTAP?" , "" , MessageBoxButton.YesNo);
            this.Visibility = Visibility.Visible;
            if (result == MessageBoxResult.Yes)
                Process.Start(
                    new ProcessStartInfo(
                        "https://raw.githubusercontent.com/wongjiahau/TTAP-UTAR/master/TTAP_Tutorial_v2.gif"));
        }
    }
}
