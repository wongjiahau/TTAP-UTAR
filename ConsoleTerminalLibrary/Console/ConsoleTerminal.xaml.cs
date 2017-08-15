using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConsoleTerminalLibrary.Console;

namespace EmbeddedConsole.Console {
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
            var dc = this.DataContext as ConsoleTerminalModel;
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
