using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using OpenSpotify.Models;
using OpenSpotify.Services.Util;
using OpenSpotify.ViewModels;
using OpenSpotify.Views;
using static OpenSpotify.Services.Util.Views;

namespace OpenSpotify.Services
{

    public class NavigationService : BaseService
    {

        public NavigationService(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
        }

        #region Fields

        private SettingsView _settingsView;
        private DownloadView _downloadView;
        private ApplicationModel _applicationModel;
        private HamburgerMenuGlyphItem _selectedItem;
        private UserControl _contentWindow;
        private SpotifyView _spotifyView;
        private SpotifyViewModel _spotifyViewModel;
        private SettingsViewModel _settingsViewModel;
        private DownloadsViewModel _downloadsViewModel;
        private YouTubeView _youTubeView;
        private YouTubeViewModel _youTubeViewModel;
        private AboutView _aboutView;
        private AboutViewModel _aboutViewModel;

        #endregion

        #region Properties

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value;
                OnPropertyChanged(nameof(ApplicationModel));
            }
        }

        public SettingsView SettingsView {
            get { return _settingsView; }
            set {
                _settingsView = value;
                OnPropertyChanged(nameof(SettingsView));
            }
        }

        public DownloadView DownloadView {
            get { return _downloadView; }
            set {
                _downloadView = value;
                OnPropertyChanged(nameof(DownloadView));
            }
        }

        public SpotifyView SpotifyView {
            get { return _spotifyView; }
            set {
                _spotifyView = value;
                OnPropertyChanged(nameof(SpotifyView));
            }
        }

        public YouTubeView YouTubeView {
            get { return _youTubeView; }
            set {
                _youTubeView = value;
                OnPropertyChanged(nameof(YouTube));
            }
        }

        public AboutView AboutView {
            get { return _aboutView; }
            set {
                _aboutView = value;
                OnPropertyChanged(nameof(AboutView));
            }
        }


        public HamburgerMenuGlyphItem SelectedItem {
            get { return _selectedItem; }
            set {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public UserControl ContentWindow {
            get { return _contentWindow; }
            set {
                _contentWindow = value;
                OnPropertyChanged(nameof(ContentWindow));
            }
        }

        public SpotifyViewModel SpotifyViewModel {
            get { return _spotifyViewModel; }
            set {
                _spotifyViewModel = value;
                OnPropertyChanged(nameof(SpotifyViewModel));
            }
        }

        public YouTubeViewModel YouTubeViewModel {
            get { return _youTubeViewModel; }
            set {
                _youTubeViewModel = value; 
                OnPropertyChanged(nameof(YouTubeViewModel));
            }
        }

        public SettingsViewModel SettingsViewModel {
            get { return _settingsViewModel; }
            set {
                _settingsViewModel = value;
                OnPropertyChanged(nameof(SettingsViewModel));
            }
        }

        public DownloadsViewModel DownloadsViewModel {
            get { return _downloadsViewModel; }
            set {
                _downloadsViewModel = value;
                OnPropertyChanged(nameof(DownloadsViewModel));
            }
        }

        public AboutViewModel AboutViewModel {
            get { return _aboutViewModel; }
            set {
                _aboutViewModel = value; 
                OnPropertyChanged(nameof(AboutViewModel));
            }
        }

        #endregion

        #region Commands

        public CommandHandler<object> ItemClickCommand {
            get {
                return new CommandHandler<object>(o => {

                    try {
                        switch (SelectedItem.Label) {

                            case nameof(Downloads):
                                ContentWindow = DownloadView;
                                DownloadsViewModel.ApplicationModel = ApplicationModel;
                                DownloadView.DataContext = DownloadsViewModel;
                                ApplicationModel.CurrentView = Downloads;
                                ApplicationModel.IsDownloadView = true;
                                ApplicationModel.IsListEmpty = Visibility.Collapsed;
                                return;

                            case nameof(Settings):
                                ContentWindow = SettingsView;
                                SettingsViewModel.ApplicationModel = ApplicationModel;
                                SettingsView.DataContext = SettingsViewModel;
                                ApplicationModel.CurrentView = Settings;
                                ApplicationModel.IsDownloadView = false;
                                ApplicationModel.IsListEmpty = Visibility.Collapsed;
                                return;

                            case nameof(YouTube):
                                ContentWindow = YouTubeView;
                                YouTubeViewModel.ApplicationModel = ApplicationModel;
                                YouTubeView.DataContext = YouTubeViewModel;
                                ApplicationModel.CurrentView = Util.Views.YouTube;
                                ApplicationModel.IsDownloadView = false;
                                ApplicationModel.IsListEmpty = Visibility.Collapsed;
                                return;

                            case nameof(Spotify):
                                ContentWindow = SpotifyView;
                                SpotifyViewModel.ApplicationModel = ApplicationModel;
                                SpotifyView.DataContext = SpotifyViewModel;
                                ApplicationModel.CurrentView = Downloads;
                                ApplicationModel.IsDownloadView = false;
                                ApplicationModel.IsListEmpty =
                                    ApplicationModel.DownloadCollection.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
                                break;

                            case nameof(About):
                                ContentWindow = AboutView;
                                break;
                        }
                    }
                    catch (Exception ex) {
                        new LogException(ex);
                    }
                });
            }
        }
        #endregion

        #region Functions

        public void InitializeNavigation() {

            try {
                SettingsView = new SettingsView();
                SettingsViewModel = new SettingsViewModel(ApplicationModel);
                SettingsView.DataContext = SettingsViewModel;

                DownloadView = new DownloadView();
                DownloadsViewModel = new DownloadsViewModel(ApplicationModel);
                DownloadView.DataContext = DownloadsViewModel;

                SpotifyView = new SpotifyView();
                SpotifyViewModel = new SpotifyViewModel(ApplicationModel);
                SpotifyView.DataContext = SpotifyViewModel;

                YouTubeView = new YouTubeView();
                YouTubeViewModel = new YouTubeViewModel(ApplicationModel);
                YouTubeView.DataContext = YouTubeViewModel;

                AboutView = new AboutView();
                AboutViewModel = new AboutViewModel(ApplicationModel);
                AboutView.DataContext = AboutViewModel;

                ContentWindow = SpotifyView;
            }
            catch (Exception ex) {
                new LogException(ex);
            }
        }
        #endregion 
    }
}
