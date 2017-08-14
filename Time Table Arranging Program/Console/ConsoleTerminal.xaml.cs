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

namespace Time_Table_Arranging_Program.Console {
    /// <summary>
    /// Interaction logic for ConsoleTerminal.xaml
    /// https://stackoverflow.com/questions/14948171/how-to-emulate-a-console-in-wpf
    /// </summary>
    public partial class ConsoleTerminal : UserControl {
        public ConsoleTerminal() {
            InitializeComponent();
        }

        private void ConsoleTerminal_OnLoaded(object sender, RoutedEventArgs e) {
            InputBlock.Focus();
        }

        private void InputBlock_OnKeyDown(object sender, KeyEventArgs e) {
            var dc = this.DataContext as ConsoleContent;
            if (e.Key == Key.Enter) {
                if (dc != null) {
                    dc.ConsoleInput = InputBlock.Text;
                    dc.RunCommand();
                }
                InputBlock.Focus();
                Scroller.ScrollToBottom();
            }
        }
    }
}
