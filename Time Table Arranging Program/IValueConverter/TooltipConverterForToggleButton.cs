using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.IValueConverter {
    public class TooltipConverterForToggleButton : System.Windows.Data.IValueConverter {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">
        /// Boolean : true or false
        /// </param>
        /// <param name="targetType"></param>
        /// <param name="parameter">
        /// A string separated by a comma (No space)
        /// E.g. "Show female student,show male student"
        /// </param>
        /// <param name="culture"></param>
        /// <returns>
        /// If value is true, return the first part of the parameter
        /// else return the second part of the parameter
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var boolvalue = (bool) value;
            var tooltips = ((string) (parameter)).Split(',');
            return boolvalue ? tooltips[0] : tooltips[1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}