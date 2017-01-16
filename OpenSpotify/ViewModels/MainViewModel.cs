using System.Windows.Controls;
using MahApps.Metro.Controls;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Views;
using static OpenSpotify.Services.Util.Views;

namespace OpenSpotify.ViewModels {

    public class MainViewModel : BaseViewModel {

        public MainViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
           
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
    }
}
