using System.Windows.Controls;
using OpenSpotify.Models;
using OpenSpotify.ViewModels;

namespace OpenSpotify.Views {
    /// <summary>
    /// Interaktionslogik für HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl {
        public HomeView(ApplicationModel applicationModel) {
            InitializeComponent();
            DataContext = new HomeViewModel(applicationModel);
        }
    }
}
