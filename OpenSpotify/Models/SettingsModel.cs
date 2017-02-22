using Newtonsoft.Json;
// ReSharper disable ExplicitCallerInfoArgument

namespace OpenSpotify.Models {

    public class SettingsModel : BaseModel {

        #region Fields

        private string _fFmpegPath;
        private double _windowTop;
        private double _windowLeft;
        private string _youtubeApiKey;
        private BitrateModel _selectedBitrate;
        private FormatModel _selectedFormat;
        private string _musicPath;
        private double _windowWidth;
        private double _windowHeight;
        private bool _deleteVideos;
        private bool _autoPlay;

        #endregion

        #region Properties

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
                SaveModelEventHandler?.Invoke();
                OnPropertyChanged(nameof(YoutubeApiKey));
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


        public BitrateModel SelectedBitrate {
            get { return _selectedBitrate; }
            set {
                _selectedBitrate = value;
                OnPropertyChanged(nameof(SelectedBitrate));
            }
        }

        public FormatModel SelectedFormat {
            get { return _selectedFormat; }
            set {
                _selectedFormat = value;
                OnPropertyChanged(nameof(SelectedFormat));
            }
        }

        public string MusicPath {
            get { return _musicPath; }
            set {
                _musicPath = value;
                OnPropertyChanged(nameof(MusicPath));
            }
        }

        public bool DeleteVideos {
            get { return _deleteVideos; }
            set {
                _deleteVideos = value; 
                OnPropertyChanged(nameof(DeleteVideos));
            }
        }

        public bool AutoPlay {
            get { return _autoPlay; }
            set {
                _autoPlay = value;
                OnPropertyChanged(nameof(AutoPlay));
            }
        }


        [JsonIgnore]
        public bool IsReady => !string.IsNullOrEmpty(FFmpegPath) && !string.IsNullOrEmpty(YoutubeApiKey);

        [JsonIgnore]
        public string StatusText => ValidateStatusText();

        private string ValidateStatusText() {
            return !string.IsNullOrEmpty(FFmpegPath) && !string.IsNullOrEmpty(YoutubeApiKey)
                ? string.Empty
                : "No FFmpeg or ApiKey";
        }

        public delegate void SaveModel();

        public static SaveModel SaveModelEventHandler;

        #endregion
    }
}
