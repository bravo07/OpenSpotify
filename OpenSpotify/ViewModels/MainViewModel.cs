using OpenSpotify.Models;
using OpenSpotify.Services;

namespace OpenSpotify.ViewModels {

    public class MainViewModel : BaseViewModel {

        public MainViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
        }

        private ApplicationModel _applicationModel;

        #region Properties

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value; 
                OnPropertyChanged(nameof(ApplicationModel));
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
    }
}
