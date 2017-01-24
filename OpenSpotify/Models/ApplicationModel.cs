using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenSpotify.Models {

    public class ApplicationModel : BaseModel {

        private SettingsModel _settings;

        public ApplicationModel() {

            if (SongCollection == null) {
                SongCollection = new ObservableCollection<SongModel>();

            }

            if (Settings == null) {
                Settings = new SettingsModel();
            }
        }

        public SettingsModel Settings {
            get { return _settings; }
            set {
                _settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        public ObservableCollection<SongModel> SongCollection { get; set; }

        public ObservableCollection<SongModel> DownloadCollection { get; set; } =
           new ObservableCollection<SongModel>();

        public List<string> DroppedSongs { get; set; } = new List<string>();
    }
}
