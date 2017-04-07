using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using OpenSpotify.Models;
using VideoLibrary;
using static OpenSpotify.Services.Util.Utils;

namespace OpenSpotify.Services {
    public class SearchService : BaseService {

        public SearchService(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
            Initialize();
        }

        #region Fields

        private string _searchQuery;
        #endregion

        #region Properties

        public ApplicationModel ApplicationModel { get; set; }

        public YouTube YouTube { get; set; }

        public YouTubeService YouTubeService { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Initializes YouTube API
        /// </summary>
        public void Initialize() {
            if(YouTubeService != null) {
                return;
            }

            YouTubeService = new YouTubeService(new BaseClientService.Initializer {
                ApiKey = ApplicationModel.Settings.YoutubeApiKey,
                ApplicationName = AppDomain.CurrentDomain.FriendlyName
            });
            YouTube = YouTube.Default;
        }

        /// <summary>
        ///     Search YouTube with given search query
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns> Returns the Completed Collection of "Songs" </returns>
        public async Task<List<SongModel>> Search(string searchQuery) {
            if(!IsInternetAvailable()) {
                ApplicationModel.StatusText = NoInternet;
                return null;
            }

            try {
                ApplicationModel.YouTubeCollection.Clear();
                var searchListRequest = YouTubeService.Search.List(SearchInfo);
                searchListRequest.Q = searchQuery;
                searchListRequest.MaxResults = 50;

                var searchListResponse = await searchListRequest.ExecuteAsync();

                return AddResults(searchListResponse);
            }
            catch(Exception ex) {
#if !DEBUG
                new LogException(ex);
#endif
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        /// <summary>
        ///     Fills the YouTube Results into SongModel Collection
        /// </summary>
        /// <param name="searchResponse"></param>
        /// <returns> Returns an Collection of SearchResults </returns>
        private static List<SongModel> AddResults(SearchListResponse searchResponse) {
            return searchResponse?.Items.Select(result => new SongModel {
                SongName = result.Snippet.Title.Length >= 50
                    ? result.Snippet.Title.Remove(50, result.Snippet.Title.Length - 50) + "..."
                    : result.Snippet.Title,
                ArtistName = result.Snippet.ChannelTitle,
                CoverImage = result.Snippet.Thumbnails.Medium.Url,
                YouTubeUri = string.IsNullOrEmpty(result.Id.VideoId) ? string.Empty : YouTubeUri + result.Id.VideoId
            }).ToList();
        }
        #endregion
    }
}