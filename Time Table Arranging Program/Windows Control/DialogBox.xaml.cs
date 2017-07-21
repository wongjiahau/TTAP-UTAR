using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Time_Table_Arranging_Program.Windows_Control {
    /// <summary>
    /// Interaction logic for DialogBox.xaml
    /// </summary>
    public partial class DialogBox : Window {
        public DialogBox() {
            InitializeComponent();
        }


        public enum ResultEnum {
            LeftButtonClicked,
            RightButtonClicked
        }

        private ResultEnum _result;


        public static ResultEnum Result;

        public static ResultEnum Show(string title , string message , string leftButtonText = "Got it!" , string rightButtonText = null) {
            var p = new DialogBox();
            p.SetContent(title , message , leftButtonText , rightButtonText);
            p.DialogHost.IsOpen = true;
            p.ShowDialog();
            Result = p._result;
            return p._result;
        }

        private void SetContent(string title , string message , string leftButtonText , string rightButtonText) {
            Button_Right.Visibility = rightButtonText == null ? Visibility.Collapsed : Visibility.Visible;
            TextBlock_Title.Text = title;
            TextBlock_Message.Text = message;
            Button_Left.Content = leftButtonText;
            Button_Right.Content = rightButtonText;
        }

        private void Button_Left_OnClick(object sender , RoutedEventArgs e) {
            _result = ResultEnum.LeftButtonClicked;
            this.Hide();
        }

        private void Button_Right_OnClick(object sender , RoutedEventArgs e) {
            _result = ResultEnum.RightButtonClicked;
            this.Hide();
        }

        private void SampleCode() {
            DialogBox.Show("Title" , "message" , "OK" , "Cancel");
            switch (DialogBox.Result) {
                case DialogBox.ResultEnum.LeftButtonClicked:
                    MessageBox.Show("You clicked left button");
                    break;
                case DialogBox.ResultEnum.RightButtonClicked:
                    MessageBox.Show("you clicked right button");
                    break;

            }
        }
    }
}


