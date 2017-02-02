using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using GongSolutions.Wpf.DragDrop;
using MahApps.Metro.Controls;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Services.Util;

namespace OpenSpotify.ViewModels {

    public class MainViewModel : BaseViewModel, IDropTarget {

        public MainViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
            Initialize();
        }

        #region Fields

        private ApplicationModel _applicationModel;
        private DownloadService _downloadService;
        private NavigationService _navigationService;
        private string _searchText;
        private CollectionView _collectionView;
        private int _selectedPageIndex;

        #endregion

        #region Properties

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value;
                OnPropertyChanged(nameof(ApplicationModel));
            }
        }

        public DownloadService DownloadService {
            get { return _downloadService; }
            set {
                _downloadService = value;
                OnPropertyChanged(nameof(DownloadService));
            }
        }

        public NavigationService NavigationService {
            get { return _navigationService; }
            set {
                _navigationService = value;
                OnPropertyChanged(nameof(NavigationService));
            }
        }

        public string SearchText {
            get { return _searchText; }
            set {
                _searchText = value;
                CollectionViewSource.GetDefaultView(ApplicationModel.SongCollection).Refresh();
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public CollectionView CollectionView {
            get { return _collectionView; }
            set {
                _collectionView = value; 
                OnPropertyChanged(nameof(CollectionView));
            }
        }

        public int SelectedPageIndex {
            get { return _selectedPageIndex; }
            set {
                _selectedPageIndex = value; 
                OnPropertyChanged(nameof(SelectedPageIndex));
            }
        }

        #endregion

        #region Commands

        public CommandHandler<object> ViewClosingCommand {
            get {
                return new CommandHandler<object>(o => {
                    ApplicationService.SaveApplicationModel(ApplicationModel);
                });
            }
        }

        public CommandHandler<object> OpenMusicCommand {
            get {
                return new CommandHandler<object>(o => {
                    if (Directory.Exists(ApplicationModel.Settings.MusicPath)) {
                        Process.Start(ApplicationModel.Settings.MusicPath);
                    }
                });
            }
        }
        #endregion

        #region Functions

        private void Initialize() {
            NavigationService = new NavigationService(ApplicationModel);
            NavigationService.InitializeNavigation();
            DownloadService = new DownloadService(ApplicationModel);
            CollectionView = (CollectionView) CollectionViewSource.GetDefaultView(ApplicationModel.SongCollection);
            CollectionView.Filter = SearchFilter;
        }

        private bool SearchFilter(object item) {
            if (string.IsNullOrEmpty(SearchText)) {
                return true;
            }
            var songModel = item as SongModel;
            return songModel != null &&
                   (songModel.SongName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                   (songModel.ArtistName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public void DragOver(IDropInfo dropInfo) {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Copy;
        }

        public async void Drop(IDropInfo dropInfo) {
            
            if (!ApplicationModel.Settings.IsReady) {
                return;
            }

            ApplicationModel.DownloadCollection.Clear();

            var dataObject = dropInfo.Data as IDataObject;
            if (dataObject != null && dataObject.GetDataPresent(DataFormats.StringFormat, true)) {

                await Task.Run(() => {
                    var filenames = (string)dataObject.GetData(DataFormats.StringFormat, true);
                    ApplicationModel.DroppedSongs = filenames?.Split('\n').ToList();

                    if (ApplicationModel.DroppedSongs == null) {
                        return;
                    }
                    
                    foreach (var droppedSong in ApplicationModel.DroppedSongs) {              
                        DownloadService.Start(droppedSong);
                    }
                });
            }
        }
        #endregion
    }
}
