using System.Windows;
using System.Windows.Controls;

namespace Time_Table_Arranging_Program.User_Control.CheckboxWithListDownMenuFolder.ErrorMessageType {
    /// <summary>
    /// Interaction logic for ClashingWithOneSubjectError.xaml
    /// </summary>
    public partial class SingleClashingError : UserControl {
        // Using a DependencyProperty as the backing store for NameOfClashingCounterpart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameOfClashingCounterpartProperty =
            DependencyProperty.Register("NameOfClashingCounterpart", typeof(string), typeof(SingleClashingError),
                new PropertyMetadata("null subject", PropertyChangedCallback));

        public SingleClashingError() {
            InitializeComponent();
        }

        public string NameOfClashingCounterpart {
            get { return (string) GetValue(NameOfClashingCounterpartProperty); }
            set { SetValue(NameOfClashingCounterpartProperty, value); }
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject,
                                                    DependencyPropertyChangedEventArgs
                                                        dependencyPropertyChangedEventArgs) {
            var d = dependencyObject as SingleClashingError;
            string newValue = dependencyPropertyChangedEventArgs.NewValue as string;
            d.NameOfClashingCounterPart.Text = newValue;
        }
    }
}