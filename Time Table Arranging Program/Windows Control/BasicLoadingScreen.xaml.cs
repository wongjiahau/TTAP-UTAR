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
using System.Windows.Shapes;

namespace Time_Table_Arranging_Program.Windows_Control {
    /// <summary>
    /// Interaction logic for BasicLoadingScreen.xaml
    /// </summary>
    public partial class BasicLoadingScreen : Window {
        public BasicLoadingScreen() {
            InitializeComponent();
        }

        public BasicLoadingScreen(string message) : this() {
            Message = message;
        }
        public string Message{
            set => MessageTextBlock.Text = value;
        }

    }
}
