using System.Windows.Controls;
using OpenSpotify.Models;
using OpenSpotify.ViewModels;

namespace OpenSpotify.Views {
    /// <summary>
    /// Interaktionslogik für DownloadView.xaml
    /// </summary>
    public partial class DownloadView : UserControl {
        public DownloadView(ApplicationModel applicationModel) {
            InitializeComponent();
            DataContext = new DownloadsViewModel(applicationModel);
        }
    }
}
