using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    ///     Interaction logic for NumberBox.xaml
    /// </summary>
    public partial class NumberBox : UserControl {
        public NumberBox() {
            InitializeComponent();
        }

        public int Number {
            get {
                if (TextBox.Text == string.Empty) return int.MaxValue;
                return int.Parse(TextBox.Text);
            }
        }

        public string NumberText {
            get { return TextBox.Text; }
            set { TextBox.Text = value; }
        }

        public event EventHandler OkButton_Clicked;

        private void OkButton_OnClick(object sender, RoutedEventArgs e) {
            OkButton_Clicked(this, null);
            OkButton.Visibility = Visibility.Hidden;
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
            foreach (var c in e.Text)
                if (!char.IsDigit(c)) {
                    e.Handled = true;
                    break;
                }
        }

        private void TextBox_OnKeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                OkButton_OnClick(this, null);
                OkButton.Focus();
            }
        }

        private void TextBox_OnGotFocus(object sender, RoutedEventArgs e) {
            OkButton.Visibility = Visibility.Visible;
        }

        private void TextBox_OnLostFocus(object sender, RoutedEventArgs e) {
            //OkButton.Visibility = Visibility.Hidden;
        }
    }
}