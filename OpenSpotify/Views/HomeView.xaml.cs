using System.Windows;
using MahApps.Metro.Controls;
using OpenSpotify.Models;
using OpenSpotify.ViewModels;
using Utils = OpenSpotify.Services.Util.Utils;

namespace OpenSpotify.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView 
    {
        public HomeView(ApplicationModel applicationModel) {
            InitializeComponent();
            this.AllowsTransparency = true;
            DataContext = new HomeViewModel(applicationModel);
        }

        private void HomeView_OnLoaded(object sender, RoutedEventArgs e) {
            Utils.EnableBlur(this);
        }
    }
}
