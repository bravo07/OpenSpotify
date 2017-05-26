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

        public CommandHandler<ItemModel> PlaySongCommand {
            get {
                return new CommandHandler<ItemModel>(selectedSong => {
                    if(!File.Exists(selectedSong.FullPath)) return;

                    var existingWindow = Application.Current.Windows.Cast<Window>().SingleOrDefault(w => w.Name == nameof(MusicView));
                    if(existingWindow != null) {
                        var viewModel = new MusicPlayerViewModel(ApplicationModel, selectedSong);
                        existingWindow.DataContext = viewModel;
                        existingWindow.Activate();
                    }
                    else {
                        CreateView(selectedSong);
                    }
                });
            }
        }

        public CommandHandler<ItemModel> OpenYoutubeCommand {
            get {
                return new CommandHandler<ItemModel>(selectedSong => {
                    if(selectedSong.YouTubeUri == null) {
                        return;
                    }

                    Process.Start(selectedSong.YouTubeUri);
                });
            }
        }


        public CommandHandler<ItemModel> OpenFileInDirectoryCommand {
            get {
                return new CommandHandler<ItemModel>(selectedSong => {
                    if(!File.Exists(selectedSong.FullPath)) {
                        ApplicationModel.SongCollection.Remove(selectedSong);
                        ApplicationService.SaveApplicationModel(ApplicationModel);
                        return;
                    }
                    Process.Start("explorer.exe", "/select, \"" + selectedSong.FullPath + "\"");
                });
            }
        }

        public CommandHandler<ItemModel> RemoveSongCommand {
            get {
                return new CommandHandler<ItemModel>(selectedSong => {
                    ApplicationModel.SongCollection.Remove(selectedSong);
                    ApplicationService.SaveApplicationModel(ApplicationModel);
                });
            }
        }

        public CommandHandler<ItemModel> RemoveAllCommand {
            get {
                return new CommandHandler<ItemModel>(selectedSong => {
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

        private void CreateView(ItemModel selectedSong) {
            MusicPlayerViewModel = new MusicPlayerViewModel(ApplicationModel, selectedSong);
            MusicView.DataContext = MusicPlayerViewModel;
            MusicView.Show();
        }

        #endregion
    }
}