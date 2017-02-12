using System.Windows;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Views;
using static OpenSpotify.Services.Util.Utils;

namespace OpenSpotify {

    public partial class App {

        private void App_OnStartup(object sender, StartupEventArgs e) {
            
            ApplicationService.InitializeApplicationDirectorys();
            var applicationModel = ApplicationService.LoadApplicationModel() ?? 
                new ApplicationModel {
                    Settings = new SettingsModel {
                        WindowHeight = 350,
                        WindowWidth = 560,
                        WindowLeft = 100,
                        WindowTop = 100,
                        MusicPath = MusicPath
                    }
                };
            var mainView = new MainView(applicationModel);
            mainView.Show();
        }
    }
}
