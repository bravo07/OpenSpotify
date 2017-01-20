using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GongSolutions.Wpf.DragDrop;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Services.Util;

namespace OpenSpotify.ViewModels {

    public class MainViewModel : BaseViewModel, IDropTarget {

        public MainViewModel() {
            Initialize();
            NavigationService = new NavigationService(ApplicationModel);
            NavigationService.InitializeNavigation();
        }


        #region Fields

        private ApplicationModel _applicationModel;
        private UserControl _contentWindow;
        private object _selectedItem;
        private DownloadService _downloadService;
        private NavigationService _navigationService; 
        #endregion

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

        #endregion

        #region Commands

        public CommandHandler<object> ViewClosingCommand {
            get {
                return new CommandHandler<object>(o => {
                    ApplicationService.SaveApplicationModel(ApplicationModel);
                });
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

        #region Functions

        private void Initialize() {
            ApplicationService.InitializeApplicationDirectorys();
            ApplicationModel = ApplicationService.LoadApplicationModel() ?? new ApplicationModel {
                Settings = new SettingsModel {
                    WindowHeight = 400,
                    WindowWidth = 600,
                    WindowTop = 250,
                    WindowLeft = 250
                }
            };
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
                    ApplicationModel.DroppedSongs?.ForEach(x => ApplicationModel.DownloadCollection.Add(new SongModel { Id = x }));

                });
            }
        }

        public void Drop(IDropInfo dropInfo) {

        }
        #endregion
    }
}
