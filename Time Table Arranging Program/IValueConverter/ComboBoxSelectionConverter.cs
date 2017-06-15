using System;
using System.Globalization;

namespace Time_Table_Arranging_Program.IValueConverter {
    public class ComboBoxSelectionConverter : System.Windows.Data.IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            bool boolValue = (bool) value;
            if (boolValue == true) return 1;
            else return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            int intValue = (int) value;
            if (intValue == 1) return true;
            else return false;
        }
    }
}