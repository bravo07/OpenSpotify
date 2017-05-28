using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenSpotify.ViewModels;

namespace OpenSpotify.Views
{
    /// <summary>
    /// Interaktionslogik für YouTubeView.xaml
    /// </summary>
    public partial class YouTubeView : UserControl
    {
        public YouTubeView() {
            InitializeComponent();
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                ((YouTubeViewModel)DataContext).SearchCommand.Execute(null);
            }
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e) {
            ButtonImage.Source = new BitmapImage(new Uri("/Assets/Search.png", UriKind.RelativeOrAbsolute));
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e) {
            ButtonImage.Source = new BitmapImage(new Uri("/Assets/SearchInactive.png", UriKind.RelativeOrAbsolute));

        }
    }
}
