using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
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
        }

        public void DragOver(IDropInfo dropInfo) {

            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Copy;
        }

        public async void Drop(IDropInfo dropInfo) {

            if (!ApplicationModel.Settings.IsReady) {
                return;
            }

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
