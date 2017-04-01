using System.Net.Mime;
using OpenSpotify.Models;
using OpenSpotify.ViewModels;

namespace OpenSpotify.Views
{
    /// <summary>
    /// Interaktionslogik für SettingView.xaml
    /// </summary>
    public partial class SettingView 
    {
        public SettingView(ApplicationModel applicationModel) {
            InitializeComponent();
            DataContext = new SettingsViewModel(applicationModel);
        }
    }
}
