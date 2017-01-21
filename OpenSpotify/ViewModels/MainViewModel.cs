using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GongSolutions.Wpf.DragDrop;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Services.Util;
using OpenSpotify.Views;

namespace OpenSpotify.ViewModels {

    public class MainViewModel : BaseViewModel, IDropTarget {

        public MainViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
            Initialize();
        }

        private ApplicationModel _applicationModel;
        private UserControl _contentWindow;
        private object _selectedItem;
        private DownloadService _downloadService;
        private DownloadView _downloadView;
        private SettingsView _settingsView;
        private HomeView _homeView;
        private NavigationService _navigationService;

        #region Properties

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value;
                OnPropertyChanged(nameof(ApplicationModel));
            }
        }

        public UserControl ContentWindow {
            get { return _contentWindow; }
            set {
                _contentWindow = value;
                OnPropertyChanged(nameof(ContentWindow));
            }
        }

        public object SelectedItem {
            get { return _selectedItem; }
            set {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public DownloadService DownloadService {
            get { return _downloadService; }
            set {
                _downloadService = value; 
                OnPropertyChanged(nameof(DownloadService));
            }
        }

        public DownloadView DownloadView {
            get { return _downloadView; }
            set {
                _downloadView = value;
                OnPropertyChanged(nameof(DownloadView));
            }
        }

        public SettingsView SettingsView {
            get { return _settingsView; }
            set {
                _settingsView = value;
                OnPropertyChanged(nameof(SettingsView));
            }
        }

        public HomeView HomeView {
            get { return _homeView; }
            set {
                _homeView = value; 
                OnPropertyChanged(nameof(HomeView));
            }
        }

        public NavigationService NavigationService {
            get { return _navigationService; }
            set {
                _navigationService = value; 
                OnPropertyChanged(nameof(NavigationService));
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

        #endregion

        #region Functions

        private void Initialize() {
            NavigationService = new NavigationService(ApplicationModel);
            NavigationService.InitializeNavigation();
        }

        public async void DragOver(IDropInfo dropInfo) {

            if (!ApplicationModel.Settings.IsReady) {
                return;
            }

            var dataObject = (dropInfo.Data as IDataObject);
            if (dataObject != null && dataObject.GetDataPresent(DataFormats.StringFormat, true)) {

                await Task.Run(() => {
                    var filenames = (string)dataObject.GetData(DataFormats.StringFormat, true);
                    ApplicationModel.DroppedSongs = filenames?.Split('\n').ToList();

                    if (DownloadService == null) {
                        DownloadService = new DownloadService(ApplicationModel);
                        DownloadService.Initialize();
                        return;
                    }

                    DownloadService.ApplicationModel = ApplicationModel;
                    DownloadService.Initialize();
                });
            }
        }

        public void Drop(IDropInfo dropInfo) {

        }
        #endregion
    }
}
