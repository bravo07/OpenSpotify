using System.Windows;
using System.Windows.Controls;
using OpenSpotify.Models;
using OpenSpotify.Services.Util;
using OpenSpotify.ViewModels;

namespace OpenSpotify.Views {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainView {

        public MainView(ApplicationModel applicationModel) {
            InitializeComponent();
            DataContext = new MainViewModel(applicationModel);
            this.AllowsTransparency = true;
        }

        private void MainView_OnLoaded(object sender, RoutedEventArgs e) {
            Utils.EnableBlur(this);

        }
    }
}
