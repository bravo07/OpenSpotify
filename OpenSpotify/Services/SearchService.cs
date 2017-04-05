using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using OpenSpotify.Models;
using OpenSpotify.Services.Util;
using VideoLibrary;
using static OpenSpotify.Services.Util.Utils;

namespace OpenSpotify.Services
{
    public class SearchService : BaseService
    {
        private string _searchQuery;

        public SearchService(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
            Initialize();
        }

        public ApplicationModel ApplicationModel { get; set; }

        public YouTube YouTube { get; set; }

        public YouTubeService YouTubeService { get; set; }

        public void Initialize() {

            if (YouTubeService != null) {
                return;
            }

            YouTubeService = new YouTubeService(new BaseClientService.Initializer {
                ApiKey = ApplicationModel.Settings.YoutubeApiKey,
                ApplicationName = AppDomain.CurrentDomain.FriendlyName
            });
            YouTube = YouTube.Default;
        }

        public async void Search(string searchQuery) {
            if (!IsInternetAvailable()) {
                ApplicationModel.StatusText = "No Internet Connection";
                return;
            }

            try {

                ApplicationModel.YouTubeCollection.Clear();
                var searchListRequest = YouTubeService.Search.List(SearchInfo);
                searchListRequest.Q = searchQuery;
                searchListRequest.MaxResults = 50;

                var searchListResponse = await searchListRequest.ExecuteAsync();
                AddResults(searchListResponse);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private void AddResults(SearchListResponse searchResponse) {
            if (searchResponse == null) {
                return;
            }

            foreach (var result in searchResponse.Items) {
                ApplicationModel.YouTubeCollection.Add(new SongModel {
                    ArtistName = result.Snippet.Title,
                    CoverImage = result.Snippet.Thumbnails.Medium.Url,
                });
            }
        }
    }
}
