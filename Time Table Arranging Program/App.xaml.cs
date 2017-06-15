using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace Time_Table_Arranging_Program {
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            DispatcherUnhandledException += OnUnhandledExeption;
        }

        private void OnUnhandledExeption(object sender, DispatcherUnhandledExceptionEventArgs e) {
            MessageBox.Show("Sorry there is an error . . .");
            e.Handled = true;
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            var comException = e.Exception as COMException;

            if (comException != null && comException.ErrorCode == -2147221040)
                e.Handled = true;
        }
    }
}