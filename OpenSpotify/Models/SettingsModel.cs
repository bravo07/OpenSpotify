namespace OpenSpotify.Models {

    public class SettingsModel : BaseModel {

        private string _fFmpegPath;
        private double _windowHeight;
        private double _windowWidth;
        private double _windowTop;
        private double _windowLeft;
        private string _youtubeApiKey;

        public string FFmpegPath {
            get { return _fFmpegPath; }
            set {
                _fFmpegPath = value;
                OnPropertyChanged(nameof(FFmpegPath));
            }
        }
        
        public string YoutubeApiKey {
            get { return _youtubeApiKey; }
            set {
                _youtubeApiKey = value;
                OnPropertyChanged(nameof(YoutubeApiKey));
            }
        }

        public double WindowHeight {
            get { return _windowHeight; }
            set {
                _windowHeight = value; 
                OnPropertyChanged(nameof(WindowHeight));
            }
        }

        public double WindowWidth {
            get { return _windowWidth; }
            set {
                _windowWidth = value;
                OnPropertyChanged(nameof(WindowWidth));
            }
        }

        public double WindowTop {
            get { return _windowTop; }
            set {
                _windowTop = value; 
                OnPropertyChanged(nameof(WindowTop));
            }
        }

        public double WindowLeft {
            get { return _windowLeft; }
            set {
                _windowLeft = value; 
                OnPropertyChanged(nameof(WindowLeft));
            }
        }
    }
}
