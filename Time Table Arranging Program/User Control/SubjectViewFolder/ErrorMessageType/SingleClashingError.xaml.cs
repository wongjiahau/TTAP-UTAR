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

namespace Time_Table_Arranging_Program.User_Control.CheckboxWithListDownMenuFolder.ErrorMessageType {
    /// <summary>
    /// Interaction logic for ClashingWithOneSubjectError.xaml
    /// </summary>
    public partial class SingleClashingError : UserControl {
        public SingleClashingError() {
            InitializeComponent();
        }

        public string NameOfClashingCounterpart {
            get { return (string)GetValue(NameOfClashingCounterpartProperty); }
            set { SetValue(NameOfClashingCounterpartProperty , value); }
        }

        // Using a DependencyProperty as the backing store for NameOfClashingCounterpart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameOfClashingCounterpartProperty =
            DependencyProperty.Register("NameOfClashingCounterpart" , typeof(string) , typeof(SingleClashingError) , new PropertyMetadata("null subject", PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
            var d = dependencyObject as SingleClashingError;
            string newValue = dependencyPropertyChangedEventArgs.NewValue as string;
            d.NameOfClashingCounterPart.Text = newValue;
        }
    }
}
