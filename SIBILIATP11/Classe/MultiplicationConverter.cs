using System;
using System.Globalization;
using System.Windows.Data;

namespace SIBILIATP11.UserControl
{
    public class MultiplicationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2)
                return 0.0;

            if (values[0] == null || values[1] == null)
                return 0.0;

            try
            {
                double value1 = System.Convert.ToDouble(values[0]);
                double value2 = System.Convert.ToDouble(values[1]);
                return value1 * value2;
            }
            catch
            {
                return 0.0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}