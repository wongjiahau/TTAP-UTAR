using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    ///     Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl {
        public event KeyEventHandler EnterKeyPressed;
        public event KeyEventHandler OnKeyPressed;
        public SearchBox() {
            InitializeComponent();
        }



        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty , value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text" , typeof(string) , typeof(SearchBox) , new PropertyMetadata(""));

        public event TextChangedEventHandler TextChanged;

        private void ClearButton_OnClick(object sender, RoutedEventArgs e) {
            TextBox.Text = "";
            ClearButton.Visibility = Visibility.Collapsed;
            TextBox.Focus();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e) {
            if (TextBox.Text.Length > 0) ClearButton.Visibility = Visibility.Visible;
            else ClearButton.Visibility = Visibility.Collapsed;
            Text = TextBox.Text;
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
            OnKeyPressed?.Invoke(this,e);
            if(e.Key == Key.Escape) TextBox.Clear();
            if (e.Key == Key.Enter) {
                EnterKeyPressed?.Invoke(null,null);
            }
        }
    }
}