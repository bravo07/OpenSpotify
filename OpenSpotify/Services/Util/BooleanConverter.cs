using System;
using System.Globalization;
using System.Windows.Data;

namespace OpenSpotify.Services.Util {

    public sealed class BooleanConverter<T> : IValueConverter {

        public T TrueValue { get; set; }
        public T FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (!(value is bool)) {
                return null;
            }
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (IsEqual(value, TrueValue)) {
                return true;
            }

            if (IsEqual(value, FalseValue)) {
                return false;
            }

            return null;
        }

        private static bool IsEqual(object x, object y) {
            if (Equals(x, y)) {
                return true;
            }

            var c = x as IComparable;
            return c?.CompareTo(y) == 0;
        }
    }

}
