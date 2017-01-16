using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OpenSpotify.ViewModels {

    public class BaseViewModel : INotifyPropertyChanged {

        private double _designWidth;
        private double _designHeight;

        public double DesignWidth {
            get { return _designWidth; }
            set {
                _designWidth = value;
                OnPropertyChanged(nameof(DesignWidth));
            }
        }

        public double DesignHeight {
            get { return _designHeight; }
            set {
                _designHeight = value;
                OnPropertyChanged(nameof(DesignHeight));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
