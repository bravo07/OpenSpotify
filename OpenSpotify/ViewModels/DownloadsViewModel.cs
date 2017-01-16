using OpenSpotify.Models;

namespace OpenSpotify.ViewModels {
    public class DownloadsViewModel : BaseViewModel {
        private ApplicationModel _applicationModel;

        public DownloadsViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
        }

        #region Properties

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value; 
                OnPropertyChanged(nameof(ApplicationModel));
            }
        }

        #endregion 
    }
}
