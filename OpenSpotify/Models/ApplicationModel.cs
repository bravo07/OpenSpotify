using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Newtonsoft.Json;
// ReSharper disable ExplicitCallerInfoArgument

namespace OpenSpotify.Models
{

    public class ApplicationModel : BaseModel
    {

        public ApplicationModel() {
            Initialize();
        }

        #region Fields

        private SettingsModel _settings;
        private bool _isDownloadView;
        private Visibility _isListEmpty;

        #endregion 

        #region Properties

        public SettingsModel Settings {
            get { return _settings; }
            set {
                _settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        public ObservableCollection<SongModel> SongCollection { get; set; }

        [JsonIgnore]
        public ObservableCollection<SongModel> DownloadCollection { get; set; } =
           new ObservableCollection<SongModel>();

        public ObservableCollection<string> DroppedSongs { get; set; } = new ObservableCollection<string>();

        [JsonIgnore]
        public bool IsDownloadView {
            get { return _isDownloadView; }
            set {
                _isDownloadView = value;
                OnPropertyChanged(nameof(IsDownloadView));
            }
        }

        public Visibility IsListEmpty {
            get { return _isListEmpty; }
            set {
                _isListEmpty = value;
                OnPropertyChanged(nameof(IsListEmpty));
            }
        }

        #endregion

        #region Functions

        private void Initialize() {
            if (SongCollection == null)
            {
                SongCollection = new ObservableCollection<SongModel> {
                    //new SongModel {SongName = "Test Song Name", ArtistName = "Test Artist Name"},
                    //new SongModel {SongName = "Test Song Name", ArtistName = "Test Artist Name"},
                    //new SongModel {SongName = "Test Song Name", ArtistName = "Test Artist Name"},
                    //new SongModel {SongName = "Test Song Name", ArtistName = "Test Artist Name"},
                };
            }

            if (Settings == null)
            {
                Settings = new SettingsModel();
            }

            DroppedSongs.CollectionChanged += (sender, args) => {
                IsListEmpty = DroppedSongs.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            };

            DownloadCollection.CollectionChanged += (sender, args) => {
                IsListEmpty = DownloadCollection.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            };
        }
        #endregion 
    }
}
