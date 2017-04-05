using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using OpenSpotify.Models;
using OpenSpotify.Services.Util;
using OpenSpotify.Views;

namespace OpenSpotify.ViewModels {

    public class HomeViewModel : BaseViewModel {

        private ApplicationModel _applicationModel;
        private MusicView _musicView;
        private MusicPlayerViewModel _musicPlayerViewModel;

        public HomeViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
            Initialize();
        }

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value;
                OnPropertyChanged(nameof(ApplicationModel));
            }
        }

        public MusicView MusicView {
            get {
                return _musicView;
            }
            set {
                _musicView = value;
                OnPropertyChanged(nameof(MusicView));
            }
        }

        public MusicPlayerViewModel MusicPlayerViewModel {
            get {
                return _musicPlayerViewModel;
            }
            set {
                _musicPlayerViewModel = value;
                OnPropertyChanged(nameof(MusicPlayerViewModel));
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

        public CommandHandler<SongModel> PlaySongCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {

                    if(selectedSong.Status != "Done!") {
                        return;
                    }

                    if (!File.Exists(selectedSong.FullPath)) {
                        return;
                    }

                    if (MusicPlayerViewModel == null) {
                        MusicPlayerViewModel = new MusicPlayerViewModel(ApplicationModel, selectedSong);
                        MusicView.DataContext = MusicPlayerViewModel;
                        MusicView.Show();
                        return;
                    }



                    if (Application.Current.Windows.Cast<Window>().All(w => w.GetType() != typeof(MusicView))) {
                        MusicPlayerViewModel = new MusicPlayerViewModel(ApplicationModel, selectedSong);
                        MusicView = new MusicView { DataContext = MusicPlayerViewModel };
                        MusicView.Show();
                        return;
                    }

                    MusicPlayerViewModel = new MusicPlayerViewModel(ApplicationModel, selectedSong);
                    MusicView.DataContext = MusicPlayerViewModel;
                });
            }
        }

        public CommandHandler<SongModel> RemoveSongCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {
                    ApplicationModel.DownloadCollection.Remove(selectedSong);
                    ApplicationModel.IsListEmpty =
                                ApplicationModel.DownloadCollection.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
                });
            }
        }

        public CommandHandler<SongModel> RemoveAllCommand {
            get {
                return new CommandHandler<SongModel>(selectedSong => {
                    ApplicationModel.DownloadCollection.Clear();
                    ApplicationModel.IsListEmpty =
                                ApplicationModel.DownloadCollection.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
                });
            }
        }
        #endregion

        #region Functions

        private void Initialize()
        {
            if (MusicView == null) {
                MusicView = new MusicView();
            }
        }

        #endregion 
    }
}
