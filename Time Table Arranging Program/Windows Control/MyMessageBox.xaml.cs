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
    /// Interaction logic for MyMessageBox.xaml
    /// </summary>
    public partial class MyMessageBox : Window {
        public enum MessageboxResult {
            Cancel,
            Action,
        }

        public MyMessageBox(string title, string message, string buttonContent) {
            InitializeComponent();
            DialogHost.IsOpen = true;
            Title.Text = title;
            Message.Text = message;
            DialogButton.Content = buttonContent;
        }

        public static void ShowOk(string title, string message, string buttonContent = "Got it!") {
            var p = new MyMessageBox(title, message, buttonContent);

            p.ShowDialog();
        }

        private void DialogButton_OnClick(object sender, RoutedEventArgs e) {
            DialogHost.IsOpen = false;
            Close();
        }
    }
}