using System;
using System.Globalization;
using System.Windows.Data;

namespace TWK.Prompter.Converters
{
    //This is really stupid but works
    public class MirrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var _mirror = (int)value;
            if (_mirror == 0)
                return 1;
            else
                return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
