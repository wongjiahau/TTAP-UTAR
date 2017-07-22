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
using Time_Table_Arranging_Program.Windows_Control;

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
            DialogBox.Show("Hello there!", "Do you want to watch tutorial of TTAP?" , "Nope" , "Watch");
            this.Visibility = Visibility.Visible;
            if (DialogBox.Result == DialogBox.ResultEnum.RightButtonClicked)
                Process.Start(
                    new ProcessStartInfo(
                        "https://raw.githubusercontent.com/wongjiahau/TTAP-UTAR/master/TTAP_Tutorial_v2.gif"));
        }
    }
}
