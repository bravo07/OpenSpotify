using OpenSpotify.Models;

namespace OpenSpotify.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private ApplicationModel _applicationModel;

        public AboutViewModel(ApplicationModel applicationModel) {
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
