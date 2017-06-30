using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    ///     Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl {
        public SearchBox() {
            InitializeComponent();
        }

        public string Text {
            get { return TextBox.Text; }
            set { TextBox.Text = value; }
        }

        public event TextChangedEventHandler TextChanged;

        private void ClearButton_OnClick(object sender, RoutedEventArgs e) {
            TextBox.Text = "";
            ClearButton.Visibility = Visibility.Collapsed;
            TextBox.Focus();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e) {
            if (TextBox.Text.Length > 0) ClearButton.Visibility = Visibility.Visible;
            else ClearButton.Visibility = Visibility.Collapsed;
            TextChanged?.Invoke(this, null);
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e) {
            TextBox.Focus();
        }


        public new bool IsKeyboardFocused() {
            return TextBox.IsKeyboardFocused;
        }


        private void SearchBox_OnGotFocus(object sender, RoutedEventArgs e) {
            TextBox.Focus();
            Keyboard.Focus(TextBox);
            e.Handled = true;
        }

        private void TextBox_OnKeyUp(object sender, KeyEventArgs e) {
            if(e.Key == Key.Escape) TextBox.Clear();
        }
    }
}