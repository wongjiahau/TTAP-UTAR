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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Time_Table_Arranging_Program.User_Control.TimeChooserCircular {
    /// <summary>
    /// Interaction logic for XorButton.xaml
    /// </summary>
    public interface IXorButton {
        void SetContent(string value1, string value2);
        string GetCurrentValue();
        void SetCurrentAsValue1();
        void SetCurrentAsValue2();
    }

    public partial class XorButton : UserControl, IXorButton {
        private readonly string[] _values = new string[2];
        private int _currentPointer;

        public XorButton() {
            InitializeComponent();
        }

        public new double FontSize {
            get { return Button.FontSize; }
            set { Button.FontSize = value; }
        }

        public void SetContent(string value1, string value2) {
            _values[0] = value1;
            _values[1] = value2;
            Button.Content = _values[0];
        }

        public string GetCurrentValue() {
            return Button.Content.ToString();
        }

        public void SetCurrentAsValue1() {
            Button.Content = _values[0];
            _currentPointer = 0;
        }

        public void SetCurrentAsValue2() {
            Button.Content = _values[1];
            _currentPointer = 1;
        }


        private void Button_OnClicked(object sender, RoutedEventArgs e) {
            _currentPointer++;
            if (_currentPointer > 1) _currentPointer = 0;
            Button.Content = _values[_currentPointer];
        }
    }
}