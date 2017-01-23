using System.Windows;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Views;

namespace OpenSpotify {

    public partial class App {

        private void App_OnStartup(object sender, StartupEventArgs e) {
            
            ApplicationService.InitializeApplicationDirectorys();
            var applicationModel = ApplicationService.LoadApplicationModel() ?? 
                new ApplicationModel {
                    Settings = new SettingsModel {
                        WindowHeight = 350,
                        WindowLeft = 100,
                        WindowTop = 100,
                        WindowWidth = 500
                    }
                };
            var mainView = new MainView(applicationModel);
            mainView.Show();
        }
    }
}
