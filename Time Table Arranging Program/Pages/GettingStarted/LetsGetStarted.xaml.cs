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
using Time_Table_Arranging_Program.Pages.Page_GettingStarted;

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