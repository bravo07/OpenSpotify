using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenSpotify.Models {
    public class SongModel : BaseModel {

        #region Fields

        private string _id;
        private string _doing;
        private string _ico;
        private string _image;
        private double _length;
        private int _progress;
        private string _songname;
        private string _youtubeid;
        private string _albumname;
        private string _iconUri;
        private string _coverImage;

        #endregion

        #region Properties

        public string Id {
            get { return _id; }
            set {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string AlbumName {
            get { return _albumname; }
            set {
                _albumname = value;
                OnPropertyChanged(nameof(AlbumName));
            }
        }

        public string SongName {
            get { return _songname; }
            set {
                _songname = value;
                OnPropertyChanged(nameof(SongName));

            }
        }

        public string Image {
            get { return _image; }
            set {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public int Progress {
            get { return _progress; }
            set {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        public string IconUri {
            get { return _iconUri; }
            set {
                _iconUri = value;
                OnPropertyChanged(nameof(IconUri));
            }
        }

        public string CoverImage {
            get { return _coverImage; }
            set {
                _coverImage = value;
                OnPropertyChanged(nameof(CoverImage));
            }
        }

        public List<string> Artists { get; set; }

        #endregion 
    }
}
