using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace OpenSpotify.Services.Util {

    public class ImageCoverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var s = value as string;
            if (s != null) {
                return new BitmapImage(new Uri(s, UriKind.RelativeOrAbsolute));
            }

            var uri = value as Uri;
            if (uri != null) {
                return new BitmapImage(uri);
            }

            return new BitmapImage();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
