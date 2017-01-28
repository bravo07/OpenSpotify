using System.Diagnostics;
using OpenSpotify.Models;
using OpenSpotify.Services.Util;

namespace OpenSpotify.ViewModels {

    public class HomeViewModel : BaseViewModel {

        private ApplicationModel _applicationModel;

        public HomeViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
        }

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value;
                OnPropertyChanged(nameof(ApplicationModel));
            }
        }

        #region Commands

        public CommandHandler<SongModel> OpenYoutubeCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {
                    if (selectedSong.YouTubeUri == null) {
                        return;
                    }

                    Process.Start(selectedSong.YouTubeUri);
                });
            }
        }

        public CommandHandler<SongModel> RemoveSongCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {
                    ApplicationModel.DownloadCollection.Remove(selectedSong);
                });
            }
        }
        #endregion
    }
}
