using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using OpenSpotify.Models;
using OpenSpotify.Services;
using OpenSpotify.Services.Util;

namespace OpenSpotify.ViewModels
{
    public class YouTubeViewModel : BaseViewModel
    {
        public YouTubeViewModel(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
            Initialize();
        }

        #region Fields

        private ApplicationModel _applicationModel;
        private string _searchText;
        private bool _isSearching;
        #endregion

        #region Properties
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

        public List<string> DownloadOptionsCollection { get; set; } = 
            new List<string>{"Mp4", "Mp3"};

        public bool IsSearching {
            get { return _isSearching; }
            set {
                _isSearching = value;
                OnPropertyChanged(nameof(IsSearching));
            }
        }

        public SearchService SearchService { get; set; }
        #endregion

        #region Commands
        
        public CommandHandler<object> SearchCommand {
            get {
                return new CommandHandler<object>(async o => {
                    IsSearching = true;
                    AddMatches(await SearchService.Search(SearchText));
                    IsSearching = false;
                });
            }
        }

        public CommandHandler<object> ClearSearchBoxCommand {
            get {
                return new CommandHandler<object>(o => {
                    ApplicationModel.YouTubeCollection.Clear();
                });
            }
        }

        public CommandHandler<SongModel> OpenYoutubeCommand {
            get {
                return new CommandHandler<SongModel>(song => {
                    Process.Start(song.YouTubeUri);
                });
            }
        }
        #endregion

        #region Functions

        private void Initialize() {
            if (SearchService == null) {
                SearchService = new SearchService(ApplicationModel);
            }
        }

        private void AddMatches(IEnumerable<SongModel> matchList) {
            foreach (var match in matchList) {
                ApplicationModel.YouTubeCollection.Add(match);
            }
        } 
        #endregion
    }
}