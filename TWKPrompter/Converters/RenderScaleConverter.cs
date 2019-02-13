using System;
using System.Globalization;
using System.Windows.Data;

namespace TWKPrompter.Converters
{
    public class RenderScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var _scale = (double)value;
            return new System.Windows.Point(_scale / 2, _scale / 2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
