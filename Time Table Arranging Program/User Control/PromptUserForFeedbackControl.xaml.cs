using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Time_Table_Arranging_Program.Properties;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    /// Interaction logic for PromptUserForFeedbackControl.xaml
    /// </summary>
    public partial class PromptUserForFeedbackControl : UserControl {
        public PromptUserForFeedbackControl() {
            InitializeComponent();
        }

        private void RateButton_OnClick(object sender, RoutedEventArgs e) {
            CheckStatusCheckboxState();
            string FeedbackFormUrl = "https://goo.gl/forms/qKdc6EVGbxspoTaS2";
            Application.Current.Shutdown();
            Process.Start(new ProcessStartInfo(FeedbackFormUrl));
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
            CheckStatusCheckboxState();
            Application.Current.Shutdown();
        }

        private void CheckStatusCheckboxState() {
            if (Checkbox.IsChecked.Value) {
                Settings.Default["PromptForFeedback"] = false;
                Settings.Default.Save();
            }
        }
    }
}