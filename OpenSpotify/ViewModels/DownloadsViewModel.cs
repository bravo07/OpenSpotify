using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Services.Util;
using OpenSpotify.Views;

namespace OpenSpotify.ViewModels {

    public class DownloadsViewModel : BaseViewModel {
        public DownloadsViewModel(ApplicationModel applicationModel) {
            Initialize();
            ApplicationModel = applicationModel;
        }

        private ApplicationModel _applicationModel;
        private MusicPlayerViewModel _musicPlayerViewModel;
        private MusicView _musicView;

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
                    if(!File.Exists(selectedSong.FullPath)) return;

                    var existingWindow = Application.Current.Windows.Cast<Window>().SingleOrDefault(w => w.Name == nameof(MusicView));
                    if(existingWindow != null) {
                        existingWindow.DataContext = new MusicPlayerViewModel(ApplicationModel, selectedSong);
                        existingWindow.Activate();
                    }
                    else {
                        CreateView(selectedSong);
                    }
                });
            }
        }

        public CommandHandler<SongModel> OpenYoutubeCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {
                    if(selectedSong.YouTubeUri == null) {
                        return;
                    }

                    Process.Start(selectedSong.YouTubeUri);
                });
            }
        }


        public CommandHandler<SongModel> OpenFileInDirectoryCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {
                    if(!File.Exists(selectedSong.FullPath)) {
                        ApplicationModel.SongCollection.Remove(selectedSong);
                        ApplicationService.SaveApplicationModel(ApplicationModel);
                        return;
                    }
                    Process.Start("explorer.exe", "/select, \"" + selectedSong.FullPath + "\"");
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
            if(MusicView == null) MusicView = new MusicView();
        }

        private void CreateView(SongModel selectedSong) {
            MusicPlayerViewModel = new MusicPlayerViewModel(ApplicationModel, selectedSong);
            MusicView.DataContext = MusicPlayerViewModel;
            MusicView.Show();
        }

        #endregion
    }
}