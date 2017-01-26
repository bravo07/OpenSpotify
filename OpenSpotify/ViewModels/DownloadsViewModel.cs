using System.Diagnostics;
using System.IO;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Services.Util;

namespace OpenSpotify.ViewModels {

    public class DownloadsViewModel : BaseViewModel {

        private ApplicationModel _applicationModel;

        public DownloadsViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
        }

        #region Properties

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value;
                OnPropertyChanged(nameof(ApplicationModel));
            }
        }

        #endregion

        #region Command

        public CommandHandler<SongModel> PlaySongCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {
                    Debug.WriteLine(selectedSong.FileName);
                    //if (File.Exists(selectedSong.FileName)) {
                    //    Process.Start(selectedSong.FileName);
                    //}
                });
            }
        }

        public CommandHandler<SongModel> OpenFileInDirectoryCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {
                    ApplicationService.SaveApplicationModel(ApplicationModel);
                });
            }
        }

        public CommandHandler<SongModel> RemoveSongCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {
                    ApplicationService.SaveApplicationModel(ApplicationModel);
                });
            }
        }
        #endregion
    }
}
