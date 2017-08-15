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

        private ConsoleTerminalModel _model;
        public void Initialize(ConsoleTerminalModel consoleTerminalModel) {
            _model = consoleTerminalModel;
            this.DataContext = _model;
        }

        private void ConsoleTerminal_OnLoaded(object sender , RoutedEventArgs e) {
            InputBlock.Focus();
        }

        private void InputBlock_OnKeyDown(object sender , KeyEventArgs e) {
            string input = InputBlock.Text;
            
            switch (e.Key) {
                case Key.Enter:
                    _model.ExecuteCommand(input);
                    break;
                case Key.Tab:
                    _model.ShowMatchingCommand(input);
                    break;
                case Key.Up:
                    _model.GoToPreviousCommand();
                    break;
                case Key.Down:
                    _model.GoToNextCommand();
                    break;
            }
            
            InputBlock.Focus();
            Keyboard.Focus(InputBlock);
            Scroller.ScrollToBottom();
        }

        private void InputBlock_OnPreviewKeyDown(object sender, KeyEventArgs e) {
            throw new System.NotImplementedException();
        }
    }
}
