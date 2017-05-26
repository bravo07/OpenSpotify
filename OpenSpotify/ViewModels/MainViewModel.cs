using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using GongSolutions.Wpf.DragDrop;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Services.Util;
using static OpenSpotify.Models.SettingsModel;
using static OpenSpotify.Services.ApplicationService;
using static OpenSpotify.Services.Util.Utils;

namespace OpenSpotify.ViewModels
{
    public class MainViewModel : BaseViewModel, IDropTarget
    {
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
        private WindowState _windowState;
        private bool _showInTaskbar;

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

        public WindowState WindowState {
            get { return _windowState; }
            set {
                _windowState = value;
                OnPropertyChanged(nameof(WindowState));
            }
        }

        public bool ShowInTaskbar {
            get { return _showInTaskbar; }
            set {
                _showInTaskbar = value;
                OnPropertyChanged(nameof(ShowInTaskbar));
            }
        }

        #endregion

        #region Commands

        public CommandHandler<object> OpenMusicCommand {
            get {
                return new CommandHandler<object>(o => {
                    if (Directory.Exists(ApplicationModel.Settings.MusicPath)) {
                        Process.Start(ApplicationModel.Settings.MusicPath);
                    }
                });
            }
        }

        public CommandHandler<Window> TaskbarDoubleClickCommand {
            get {
                return new CommandHandler<Window>(window => {
                    Application.Current.Dispatcher.Invoke(() => {
                        window.Show();
                        window.WindowState = WindowState.Normal;
                        ShowInTaskbar = true;
                    });
                });
            }
        }

        public CommandHandler<object> CloseAppCommand {
            get {
                return new CommandHandler<object>(o => {
                    SaveApplicationModel(ApplicationModel);
                    Application.Current.Shutdown();
                });
            }
        }

        public CommandHandler<object> ShowCommand {
            get {
                return new CommandHandler<object>(o => {
                    WindowState = WindowState.Normal;
                    ShowInTaskbar = true;
                });
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Initializes ViewModel
        /// </summary>
        private void Initialize() {
            try {
                NavigationService = new NavigationService(ApplicationModel);
                NavigationService.InitializeNavigation();
                DownloadService = new DownloadService(ApplicationModel, NavigationService);
                CollectionView = (CollectionView)CollectionViewSource.GetDefaultView(ApplicationModel.SongCollection);
                CollectionView.Filter = SearchFilter;
                ApplicationModel.StatusText = "Ready...";
                ApplicationModel.IsListEmpty = Visibility.Visible;
                NavigationService.ContentWindow = NavigationService.SpotifyView;
                ShowInTaskbar = true;

                SaveModelEventHandler += () => {
                    SaveApplicationModel(ApplicationModel);
                };
            }
            catch (Exception ex) {
#if !DEBUG
                new LogException(ex);
#endif
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private bool SearchFilter(object item) {
            if (string.IsNullOrEmpty(SearchText)) {
                return true;
            }

            var songModel = item as ItemModel;
            return songModel != null &&
                   (songModel.SongName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                   (songModel?.ArtistName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public void DragOver(IDropInfo dropInfo) {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Copy;
        }

        /// <summary>
        /// Drag & Drop Handler 
        /// </summary>
        /// <param name="dropInfo"></param>
        public async void Drop(IDropInfo dropInfo) {

            try {

                if (!ApplicationModel.Settings.IsReady) {
                    ApplicationModel.StatusText = NotReady;
                    return;
                }

                ApplicationModel.DownloadCollection.Clear();
                ApplicationModel.DroppedSongs.Clear();

                NavigationService.ContentWindow = NavigationService.SpotifyView;

                var dataObject = dropInfo.Data as IDataObject;
                if (dataObject != null && dataObject.GetDataPresent(DataFormats.StringFormat, true)) {

                    await Task.Run(() => {

                        var filenames = (string)dataObject.GetData(DataFormats.StringFormat, true);
                        ToCollection(filenames?.Split('\n'), ApplicationModel.DroppedSongs);
                        ApplicationModel.StatusText = $"{ApplicationModel.DroppedSongs.Count} Dropped...";
                        if (ApplicationModel.DroppedSongs == null) {
                            return;
                        }

                        for (var i = 0; i < ApplicationModel.DroppedSongs.Count; i++) {
                            DownloadService.Start(ApplicationModel.DroppedSongs[i]);
                            ApplicationModel.StatusText = $"{i}/{ApplicationModel.DroppedSongs.Count} Done.";
                        }
                        ApplicationModel.StatusText =
                            $"Downloading {ApplicationModel.DroppedSongs.Count}/{ApplicationModel.DroppedSongs.Count}";
                    });
                }
            }
            catch (Exception ex) {
#if !DEBUG
                new LogException(ex);
#endif
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private void ToCollection(IEnumerable<string> split, ObservableCollection<string> applicationModelDroppedSongs) {
            foreach (var songId in split) {
                applicationModelDroppedSongs.Add(songId);
            }
            ApplicationModel.DroppedSongs = applicationModelDroppedSongs;
        }

        public void ViewClosing(object sender, CancelEventArgs e) {
            SaveApplicationModel(ApplicationModel);
            WindowState = WindowState.Minimized;
            e.Cancel = true;
            ShowInTaskbar = false;

            if (ApplicationModel.Settings.DeleteVideos) {
                ClearTemp();
            }
        }



        #endregion
    }
}