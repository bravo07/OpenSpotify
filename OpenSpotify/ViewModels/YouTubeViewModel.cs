using System.Collections.ObjectModel;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Services.Util;

namespace OpenSpotify.ViewModels
{
    public class YouTubeViewModel : BaseViewModel
    {
        private ApplicationModel _applicationModel;
        private string _searchText;
        private bool _isSearching;

        public YouTubeViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
            Initialize();
        }

        public ApplicationModel ApplicationModel {
            get { return _applicationModel; }
            set {
                _applicationModel = value;
                OnPropertyChanged(nameof(ApplicationModel));
            }
        }

        public string SearchText {
            get { return _searchText; }
            set {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public ObservableCollection<SongModel> DebugCollection { get; set; }

        public bool IsSearching {
            get { return _isSearching; }
            set {
                _isSearching = value;
                OnPropertyChanged(nameof(IsSearching));
            }
        }

        public SearchService SearchService { get; set; }
        #region Commands

        public CommandHandler<object> DownloadCommand {
            get {
                return new CommandHandler<object>(o => {
                    SearchService.Search(SearchText);
                });
            }
        }

        #endregion 

        private void Initialize() {

            if (SearchService == null) {
                SearchService = new SearchService(ApplicationModel);
            }



            //DebugCollection = new ObservableCollection<SongModel> {
            //    new SongModel {
            //        SongName = "Test SongName",
            //        YouTubeUri = "http://www.adasdasd.com/asdasdasd",
            //        Status = "Downloading",
            //        ArtistName = "TestArtist"
            //    },
            //    new SongModel {
            //        SongName = "Test SongName",
            //        YouTubeUri = "http://www.adasdasd.com/asdasdasd",
            //        Status = "Downloading",
            //        ArtistName = "TestArtist"
            //    },new SongModel {
            //        SongName = "Test SongName",
            //        YouTubeUri = "http://www.adasdasd.com/asdasdasd",
            //        Status = "Downloading",
            //        ArtistName = "TestArtist"
            //    },
            //    new SongModel {
            //        SongName = "Test SongName",
            //        YouTubeUri = "http://www.adasdasd.com/asdasdasd",
            //        Status = "Downloading",
            //        ArtistName = "TestArtist"
            //    },
            //};
        }
    }
}