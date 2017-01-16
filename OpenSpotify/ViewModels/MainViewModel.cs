using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GongSolutions.Wpf.DragDrop;
using MahApps.Metro.Controls;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Views;
using static OpenSpotify.Services.Util.Views;

namespace OpenSpotify.ViewModels {

    public class MainViewModel : BaseViewModel, IDropTarget {

        public MainViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
            Initialize();
        }

        private ApplicationModel _applicationModel;
        private UserControl _contentWindow;
        private object _selectedItem;

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

        #endregion

        #region Commands

        public CommandHandler<object> ViewClosingCommand {
            get {
                return new CommandHandler<object>(o => {
                    ApplicationService.SaveApplicationModel(ApplicationModel);
                });
            }
        }

        public CommandHandler<object> ItemClickCommand {
            get {
                return new CommandHandler<object>(o => {
                    switch (((HamburgerMenuGlyphItem)SelectedItem).Label) {
                        case nameof(Downloads):
                            ContentWindow = new DownloadView(ApplicationModel);
                            return;
                        case nameof(Settings):
                            ContentWindow = new SettingsView(ApplicationModel);
                            return;
                        case nameof(Home):
                            ContentWindow = new HomeView(ApplicationModel);
                            break;
                    }
                });
            }
        }

        #endregion

        #region Functions

        private void Initialize() {
            ContentWindow = new HomeView(ApplicationModel);
        }

        public async void DragOver(IDropInfo dropInfo) {

            if (!ApplicationModel.Settings.IsReady) {
                return;
            }

            var dataObject = (dropInfo.Data as IDataObject);
            if (dataObject != null && dataObject.GetDataPresent(DataFormats.StringFormat, true)) {

                await Task.Run(() => {
                    var filenames = (string)dataObject.GetData(DataFormats.StringFormat, true);
                    var spotifyFiles = filenames?.Split('\n');

                    foreach (var file in spotifyFiles) {
                        var songObj = DownloadService.DownloadSongInformation(file);
                        if (songObj == null) {
                            if (!ApplicationModel.FailedSongCollection.Contains(file)) {
                                ApplicationModel.FailedSongCollection.Add(file);
                            }
                        }
                        else {
                            Application.Current.Dispatcher.Invoke(delegate {
                                if (ApplicationModel.DownloadCollection.All(i => i.Id != songObj.Id)) {
                                    ApplicationModel.DownloadCollection.Add(songObj);
                                }
                            });
                        }
                    }
                });
            }
        }

        public void Drop(IDropInfo dropInfo) {

        }
        #endregion

    }
}
