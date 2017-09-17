using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Time_Table_Arranging_Program.MVVM_Framework.Views {
    /// <summary>
    /// Interaction logic for CyclicIndexView.xaml
    /// </summary>
    public partial class CyclicIndexView : UserControl {
        public CyclicIndexView() {
            InitializeComponent();
        }
    }

    public class IndexViewerVisibilityConverter : System.Windows.Data.IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if ((int) value < 0) {
                return Visibility.Hidden;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}