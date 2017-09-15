using System;
using System.Globalization;
using System.Windows;

namespace Time_Table_Arranging_Program.IValueConverter {
    public class VisibilityConverter : System.Windows.Data.IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (parameter != null) {
                if (value == null || (bool) value == false) {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
            if (value == null || (bool) value == false) {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}