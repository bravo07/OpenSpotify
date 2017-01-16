using OpenSpotify.Models;

namespace OpenSpotify.ViewModels {
    public class HomeViewModel : BaseViewModel {

        private ApplicationModel _applicationModel;

        public HomeViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
        }

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value; 
                OnPropertyChanged(nameof(ApplicationModel));
            }
        }
    }
}
