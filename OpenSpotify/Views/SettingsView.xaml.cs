using System.Windows.Controls;
using OpenSpotify.Models;
using OpenSpotify.ViewModels;

namespace OpenSpotify.Views {
    /// <summary>
    /// Interaktionslogik für SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl {
        public SettingsView(ApplicationModel applicationModel) {
            InitializeComponent();
            DataContext = new SettingsViewModel(applicationModel);
        }
    }
}
