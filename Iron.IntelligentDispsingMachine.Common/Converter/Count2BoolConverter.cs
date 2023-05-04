using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Iron.IntelligentDispsingMachine.Common.Converter
{
    public class Count2BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return false;
                }
                else
                {
                    IEnumerable<object> list = value as IEnumerable<object>;
                    if (list.Count() > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
