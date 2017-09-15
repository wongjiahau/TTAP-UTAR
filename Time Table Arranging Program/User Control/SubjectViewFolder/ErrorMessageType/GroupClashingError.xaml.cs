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