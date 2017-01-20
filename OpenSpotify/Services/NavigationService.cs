using System.Windows.Controls;
using MahApps.Metro.Controls;
using OpenSpotify.Models;
using OpenSpotify.Services.Util;
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
        #endregion

        #region Commands

        public CommandHandler<object> ItemClickCommand {
            get {
                return new CommandHandler<object>(o => {
                    switch (SelectedItem.Label) {
                        case nameof(Downloads):
                            ContentWindow = DownloadView;
                            return;
                        case nameof(Settings):
                            ContentWindow = SettingsView;
                            return;
                        case nameof(Home):
                            ContentWindow = HomeView;
                            break;
                    }
                });
            }
        }
        #endregion

        #region Functions

        public void InitializeNavigation() {

            SettingsView = new SettingsView(ApplicationModel);
            DownloadView = new DownloadView(ApplicationModel);
            HomeView = new HomeView(ApplicationModel);
            ContentWindow = HomeView;
        }
        #endregion 
    }
}
