using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.IValueConverter {
    public class CutoffConverter : System.Windows.Data.IValueConverter {
        public object Convert(object value , Type targetType , object parameter , CultureInfo culture) {
            int cutoff = int.Parse((string)parameter);
            return ((int)value) > cutoff;
        }

        public object ConvertBack(object value , Type targetType , object parameter , CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
