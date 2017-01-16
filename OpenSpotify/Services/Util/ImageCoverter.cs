using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace OpenSpotify.Services.Util {
    public class ImageCoverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is string)
                return new BitmapImage(new Uri((string)value, UriKind.RelativeOrAbsolute));

            if (value is Uri)
                return new BitmapImage((Uri)value);

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
