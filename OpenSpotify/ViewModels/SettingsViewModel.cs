using System;
using Microsoft.Win32;
using OpenSpotify.Models;
using OpenSpotify.Services;

namespace OpenSpotify.ViewModels {

    public class SettingsViewModel : BaseViewModel {

        private ApplicationModel _applicationModel;

        public SettingsViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
        }

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value; 
                OnPropertyChanged(nameof(SettingsViewModel));
            }
        }

        public CommandHandler<object> FFmpegPathCommand {
            get {
                return new CommandHandler<object>(o => {
                    var openFileDialog = new OpenFileDialog {
                        Filter = "Executable Files |*.exe;",
                        InitialDirectory =
                            Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                    };
                    if (openFileDialog.ShowDialog() == true) {
                        ApplicationModel.Settings.FFmpegPath = openFileDialog.FileName;
                    }
                });
            }
        }
    }
}
