using System.Windows.Controls;
using MahApps.Metro.Controls;
using OpenSpotify.Models;
using OpenSpotify.Services.Util;
using OpenSpotify.ViewModels;
using OpenSpotify.Views;
using static OpenSpotify.Services.Util.Views;

namespace OpenSpotify.Services {

    public class NavigationService : BaseService {

        public NavigationService(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
        }

        #region Fields

        private SettingsView _settingsView;
        private DownloadView _downloadView;
        private ApplicationModel _applicationModel;
        private HamburgerMenuGlyphItem _selectedItem;
        private UserControl _contentWindow;
        private HomeView _homeView;
        private HomeViewModel _homeViewModel;
        private SettingsViewModel _settingsViewModel;
        private DownloadsViewModel _downloadsViewModel;

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

        public HomeView HomeView {
            get { return _homeView; }
            set {
                _homeView = value;
                OnPropertyChanged(nameof(HomeView));
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

        public HomeViewModel HomeViewModel {
            get { return _homeViewModel; }
            set {
                _homeViewModel = value; 
                OnPropertyChanged(nameof(HomeViewModel));
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

        #endregion

        #region Commands

        public CommandHandler<object> ItemClickCommand {
            get {
                return new CommandHandler<object>(o => {

                    switch (SelectedItem.Label) {

                        case nameof(Downloads):
                            ContentWindow = DownloadView;
                            DownloadsViewModel.ApplicationModel = ApplicationModel;
                            DownloadView.DataContext = DownloadsViewModel;
                            ApplicationModel.IsDownloadView = true;
                            return;

                        case nameof(Settings):
                            ContentWindow = SettingsView;
                            SettingsViewModel.ApplicationModel = ApplicationModel;
                            SettingsView.DataContext = SettingsViewModel;
                            ApplicationModel.IsDownloadView = false;
                            return;

                        case nameof(Home):
                            ContentWindow = HomeView;
                            HomeViewModel.ApplicationModel = ApplicationModel;
                            HomeView.DataContext = HomeViewModel;
                            ApplicationModel.IsDownloadView = false;
                            break;
                    }
                });
            }
        }
        #endregion

        #region Functions

        public void InitializeNavigation() {

            SettingsView = new SettingsView();
            SettingsViewModel = new SettingsViewModel(ApplicationModel);
            SettingsView.DataContext = SettingsViewModel;

            DownloadView = new DownloadView();
            DownloadsViewModel = new DownloadsViewModel(ApplicationModel);
            DownloadView.DataContext = DownloadsViewModel;

            HomeView = new HomeView();
            HomeViewModel = new HomeViewModel(ApplicationModel);
            HomeView.DataContext = HomeViewModel;

            ContentWindow = HomeView;
        }
        #endregion 
    }
}
