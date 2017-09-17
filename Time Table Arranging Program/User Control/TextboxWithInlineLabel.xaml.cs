using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    /// Interaction logic for TextboxWithInlineLabel.xaml
    /// </summary>
    public partial class TextboxWithInlineLabel : UserControl {
        public TextboxWithInlineLabel() {
            InitializeComponent();
        }

        public string Text {
            get { return TextBox.Text; }
            set { TextBox.Text = value; }
        }

        public event TextChangedEventHandler TextChanged;

        public new void Focus() {
            TextBox.Focus();
        }

        private void Label_OnMouseDown(object sender, MouseButtonEventArgs e) {
            TextBox.Focus();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e) {
            if (TextBox.Text.Length == 0) {
                Label.Visibility = Visibility.Visible;
            }
            else {
                Label.Visibility = Visibility.Collapsed;
            }
            TextChanged(this, null);
        }

        #region Property_InlineLabelText

        public static readonly DependencyProperty SetTextProperty =
            DependencyProperty.Register("InlineLabelText", typeof(string), typeof(TextboxWithInlineLabel), new
                PropertyMetadata("", OnInlineLabelTextChanged));

        public string InlineLabelText {
            get { return (string) GetValue(SetTextProperty); }
            set { SetValue(SetTextProperty, value); }
        }

        private static void OnInlineLabelTextChanged(DependencyObject d,
                                                     DependencyPropertyChangedEventArgs e) {
            var x = d as TextboxWithInlineLabel;
            x.OnInlineLabelTextChanged(e);
        }

        private void OnInlineLabelTextChanged(DependencyPropertyChangedEventArgs e) {
            Label.Content = e.NewValue.ToString();
        }

        #endregion

        #region Property_FontSize

        public static readonly DependencyProperty SetFontProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(TextboxWithInlineLabel), new
                PropertyMetadata(10.0, OnFontChanged));

        public new double FontSize {
            get { return (double) GetValue(SetFontProperty); }
            set { SetValue(SetFontProperty, value); }
        }

        private static void OnFontChanged(DependencyObject d,
                                          DependencyPropertyChangedEventArgs e) {
            var x = d as TextboxWithInlineLabel;
            x.OnFontChanged(e);
        }

        private void OnFontChanged(DependencyPropertyChangedEventArgs e) {
            TextBox.FontSize = (double) e.NewValue;
            Label.FontSize = (double) e.NewValue;
        }

        #endregion
    }
}