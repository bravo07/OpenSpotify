using System;
using System.Globalization;
using System.Windows.Data;

namespace OpenSpotify.Services.Util {

    public class BoolToValueConverter<T> : IValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null) {
                return FalseValue;
            }
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(TrueValue) ?? false;
        }
    }

    public class PlayPauseConverter : BoolToValueConverter<string> { }
    public class SoundConverter : BoolToValueConverter<string> { }

}
