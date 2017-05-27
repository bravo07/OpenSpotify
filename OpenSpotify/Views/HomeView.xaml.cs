using System.Windows;
using OpenSpotify.Services.Util;

namespace OpenSpotify.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView 
    {
        public HomeView() {
            InitializeComponent();
            this.AllowsTransparency = true;
        }

        private void HomeView_OnLoaded(object sender, RoutedEventArgs e) {
            Utils.EnableBlur(this);
        }
    }


}
