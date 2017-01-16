using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenSpotify.Models {

    public class ApplicationModel : BaseModel {

        private SettingsModel _settings;

        public ApplicationModel() {

            if (SongCollection == null) {
                SongCollection = new ObservableCollection<SongModel> {
                    //new SongModel {SongName = "TestSong1", CoverImage = "https://recordhounds.files.wordpress.com/2014/10/album-cover.jpeg"}, // UI Test 
                    //new SongModel {SongName = "TestSong2", CoverImage = "https://recordhounds.files.wordpress.com/2014/10/album-cover.jpeg"},
                    //new SongModel {SongName = "TestSong3", CoverImage = "https://recordhounds.files.wordpress.com/2014/10/album-cover.jpeg"}
                };

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

        public List<string> FailedSongCollection { get; set; } =
            new List<string>();
    }
}
