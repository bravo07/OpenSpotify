using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using OpenSpotify.Models;
using VideoLibrary;

namespace OpenSpotify.Services {

    public class BaseService : INotifyPropertyChanged {

        public ApplicationModel ApplicationModel { get; set; }

        public YouTube YouTube { get; set; }

        public YouTubeService YouTubeService { get; set; }

        public void InitializeYouTubeApi() {
            if (YouTubeService != null) {
                return;
            }

            YouTubeService = new YouTubeService(new BaseClientService.Initializer {
                ApiKey = ApplicationModel.Settings.YoutubeApiKey,
                ApplicationName = AppDomain.CurrentDomain.FriendlyName
            });
            YouTube = YouTube.Default;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
