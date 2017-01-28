using System.Collections.Generic;
// ReSharper disable ExplicitCallerInfoArgument

namespace OpenSpotify.Models {

    public class SongModel : BaseModel {

        #region Fields

        private string _id;
        private string _image;
        private string _songname;
        private string _albumname;
        private string _coverImage;
        private string _status;
        private string _youTubeUri;
        private string _artistName;
        private double _statusValue;
        private string _fileName;
        private string _fullPath;

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

        public string CoverImage {
            get { return _coverImage; }
            set {
                _coverImage = value;
                OnPropertyChanged(nameof(CoverImage));
            }
        }

        public string Status {
            get { return _status; }
            set {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public double StatusValue {
            get { return _statusValue; }
            set {
                _statusValue = value;
                OnPropertyChanged(nameof(StatusValue));
            }
        }

        public string YouTubeUri {
            get { return _youTubeUri; }
            set {
                _youTubeUri = value;
                OnPropertyChanged(nameof(YouTubeUri));
            }
        }

        public string FullPath {
            get { return _fullPath; }
            set {
                _fullPath = value;
                OnPropertyChanged(nameof(FullPath));
            }
        }

        public List<string> Artists { get; set; }

        public string ArtistName {
            get { return _artistName; }
            set {
                _artistName = value;
                OnPropertyChanged(nameof(ArtistName));
            }
        }

        public string FileName {
            get { return _fileName; }
            set {
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        #endregion 
    }
}
