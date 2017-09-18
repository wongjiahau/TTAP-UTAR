using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace ExtraTools {
    /// <summary>
    ///     Interaction logic for DialogBox.xaml
    /// </summary>
    public partial class DialogBox : Window {
        public enum Result_ {
            LeftButtonClicked,
            RightButtonClicked
        }

        public static Result_ Result;

        private static DialogBox _lastOpenedDialog;

        private Result_ _result;

        public DialogBox() {
            InitializeComponent();
            AllowsTransparency = true;
        }

        public static object ReturnedValue { get; set; }

        public static Result_ Show(string title, string message, string leftButtonText = "Got it!",
                                      string rightButtonText = null) {
            CloseDialog();
            var p = new DialogBox();
            _lastOpenedDialog = p;
            p.SetContent(title, message, leftButtonText, rightButtonText);
            p.DialogHost.IsOpen = true;
            p.ShowDialog();
            Result = p._result;
            return p._result;
        }

        private void SetContent(string title, string message, string leftButtonText, string rightButtonText) {
            Button_Right.Visibility = rightButtonText == null ? Visibility.Collapsed : Visibility.Visible;
            TextBlock_Title.Text = title;
            TextBlock_Message.Text = message;
            Button_Left.Content = leftButtonText;
            Button_Right.Content = rightButtonText;
            Custom.Visibility = Visibility.Collapsed;
            Default.Visibility = Visibility.Visible;
        }

        public static void Show(UserControl content) {
            CloseDialog();
            var p = new DialogBox();
            _lastOpenedDialog = p;
            p.SetContent(content);
            p.DialogHost.IsOpen = true;
            if (p.Visibility == Visibility.Visible) return;
            p.ShowDialog();
        }

        public static void CloseDialog() {
            if (_lastOpenedDialog == null) return;
            _lastOpenedDialog.DialogHost.IsOpen = false;
            _lastOpenedDialog.Hide();
        }

        private void SetContent(UserControl content) {
            Default.Visibility = Visibility.Collapsed;
            Custom.Visibility = Visibility.Visible;
            Frame.Navigate(content);
        }

        private void Button_Left_OnClick(object sender, RoutedEventArgs e) {
            _result = Result_.LeftButtonClicked;
            Close();
        }

        private void Button_Right_OnClick(object sender, RoutedEventArgs e) {
            _result = Result_.RightButtonClicked;
            Close();
        }

        private void SampleCode() {
            Show("Title", "message", "OK", "Cancel");
            switch (Result) {
                case Result_.LeftButtonClicked:
                    MessageBox.Show("You clicked left button");
                    break;
                case Result_.RightButtonClicked:
                    MessageBox.Show("you clicked right button");
                    break;
            }
        }

        private void DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventargs) {
            Close();
        }

        private void DialogBox_OnClosing(object sender, CancelEventArgs e) {
            DialogHost.IsOpen = false;
        }
    }
}