using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.ConfigFileManager;
using Time_Table_Arranging_Program.Pages.Login;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program {
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            DispatcherUnhandledException += OnUnhandledExeption;
            var dataManger = new DataManager();
            dataManger.CreateDirectory();
        }

        private void OnUnhandledExeption(object sender , DispatcherUnhandledExceptionEventArgs e) {
            BugReportSender.SendIssue(e.Exception , Global.LoadedHtml);
            DialogBox.Show("TTAP has crashed due to some issue . . ." ,
                "Sorry for the inconvenience. We will fixed the issue after we received the bug report." ,
                "Nevermind" ,
                "Report the bug");
            if (DialogBox.Result == DialogBox.ResultEnum.RightButtonClicked) {
                Process.Start(new ProcessStartInfo(new UrlProvider().ReportBugUrl));
            }
            e.Handled = true;
            Current.Shutdown();
        }
    }
}