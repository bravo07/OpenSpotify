using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSpotify.Models;
using OpenSpotify.Services.Util;
using VideoLibrary;
using static OpenSpotify.Services.Util.Utils;

namespace OpenSpotify.Services {
    public class SearchService : BaseService {

        public SearchService(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
            InitializeYouTubeApi();
        }

        #region Fields

        private string _searchQuery;
        #endregion

        #region Properties

       



        #endregion

        #region YouTube

        /// <summary>
        ///     Search YouTube with given search query
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns> Returns the Completed Collection of "Songs" </returns>
        public async Task<List<ItemModel>> Search(string searchQuery) {
            if(!IsInternetAvailable()) {
                ApplicationModel.StatusText = NoInternet;
                return null;
            }

            try {
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
        private static List<ItemModel> AddResults(SearchListResponse searchResponse) {
            return searchResponse?.Items.Select(result => new ItemModel {
                SongName = result.Snippet.Title.Length >= 50
                    ? result.Snippet.Title.Remove(50, result.Snippet.Title.Length - 50) + "..."
                    : result.Snippet.Title,
                ArtistName = result.Snippet.ChannelTitle,
                CoverImage = result.Snippet.Thumbnails.Medium.Url,
                YouTubeUri = string.IsNullOrEmpty(result.Id.VideoId) ? string.Empty : YouTubeUri + result.Id.VideoId
            }).ToList();
        }
        #endregion

        #region Spotify

        public ItemModel SearchSpotifySongInformation(string id) {
            try {
                string loadedData;
                using (var webClient = new WebClient()) {
                    loadedData = webClient.DownloadString(SongInformationUri + PrepareId(id));
                }

                var desirializedInfo = JsonConvert.DeserializeObject<JObject>(loadedData);
                var songModel = new ItemModel {
                    Id = PrepareId(id),
                    Artists = new List<string>(),
                    AlbumName = (string)desirializedInfo["album"]["name"],
                    SongName = (string)desirializedInfo["name"],
                    CoverImage = (string)desirializedInfo["album"]["images"][0]["url"],
                    StatusValue = 60
                };

                var artistBuilder = new StringBuilder();
                foreach (var artist in desirializedInfo["artists"]) {
                    songModel.Artists.Add((string)artist["name"]);
                    artistBuilder.Append($"{artist["name"]},");
                }
                songModel.ArtistName = artistBuilder.ToString();
                return songModel;
            }
            catch (Exception ex) {
#if !DEBUG
                new LogException(ex);
#endif
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        #endregion 
    }
}