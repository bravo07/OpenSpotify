using System.Diagnostics;
using System.IO;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Services.Util;
using OpenSpotify.Views;

namespace OpenSpotify.ViewModels {

    public class DownloadsViewModel : BaseViewModel {

        private ApplicationModel _applicationModel;
        private MusicView _musicView;
        private MusicPlayerViewModel _musicPlayerViewModel;

        public DownloadsViewModel(ApplicationModel applicationModel) {
            Initialize();
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

        public MusicView MusicView {
            get { return _musicView; }
            set {
                _musicView = value;
                OnPropertyChanged(nameof(MusicView));
            }
        }

        public MusicPlayerViewModel MusicPlayerViewModel {
            get { return _musicPlayerViewModel; }
            set {
                _musicPlayerViewModel = value; 
                OnPropertyChanged(nameof(MusicPlayerViewModel));
            }
        }

        #endregion

        #region Command

        public CommandHandler<SongModel> PlaySongCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {

                    if (!File.Exists(selectedSong.FullPath)) {
                        return;
                    }

                    if (MusicPlayerViewModel == null) {
                        MusicPlayerViewModel = new MusicPlayerViewModel(ApplicationModel, selectedSong);
                        MusicView.DataContext = MusicPlayerViewModel;
                        MusicView.Show();
                        return;
                    }
                    else {
                        MusicPlayerViewModel = new MusicPlayerViewModel(ApplicationModel, selectedSong);
                        MusicView.DataContext = MusicPlayerViewModel;
                    }
                });
            }
        }

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


        public CommandHandler<SongModel> OpenFileInDirectoryCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {
                    if (!File.Exists(selectedSong.FullPath)) {
                        ApplicationModel.SongCollection.Remove(selectedSong);
                        ApplicationService.SaveApplicationModel(ApplicationModel);
                        return;
                    }
                    Process.Start("explorer.exe","/select, \"" + selectedSong.FullPath + "\"");
                });
            }
        }

        public CommandHandler<SongModel> RemoveSongCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {
                    ApplicationModel.SongCollection.Remove(selectedSong);
                    ApplicationService.SaveApplicationModel(ApplicationModel);
                });
            }
        }

        public CommandHandler<SongModel> RemoveAllCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {
                    ApplicationModel.SongCollection.Clear();
                    ApplicationService.SaveApplicationModel(ApplicationModel);
                });
            }
        }
        #endregion

        #region Functions

        private void Initialize() {
            if (MusicView == null) {
                MusicView = new MusicView();
            }
        }

        #endregion 
    }
}
