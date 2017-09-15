using System;
using System.Globalization;

namespace Time_Table_Arranging_Program.IValueConverter {
    public class IsStringEmptyConverter : System.Windows.Data.IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (string) value == "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}