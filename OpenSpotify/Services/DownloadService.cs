using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSpotify.Models;
using VideoLibrary;
using static OpenSpotify.Services.Util.Utils;

namespace OpenSpotify.Services {

    public class DownloadService : INotifyPropertyChanged{
        private YouTubeService _youTubeService;
        private YouTube _youTube;

        public DownloadService(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
            InitalizeYouTubeService();
            InitializeFileWatcher();
        }

        #region Properties

        public ApplicationModel ApplicationModel { get; set; }

        public YouTubeService YouTubeService {
            get { return _youTubeService; }
            set {
                _youTubeService = value; 
                OnPropertyChanged(nameof(YouTubeService));
            }
        }

        public YouTube YouTube {
            get { return _youTube; }
            set {
                _youTube = value;
                OnPropertyChanged(nameof(YouTube));
            }
        }

        public ConvertService ConvertService { get; set; }

        public FileSystemWatcher FileSystemDownloadWatcher { get; set; }

        public FileSystemWatcher FileSystemMusicWatcher { get; set; }
        #endregion 

        #region Initialization 

        private void InitalizeYouTubeService() {
            YouTubeService = new YouTubeService(new BaseClientService.Initializer {
                ApiKey = ApplicationModel.Settings.YoutubeApiKey,
                ApplicationName = AppDomain.CurrentDomain.FriendlyName
            });
            YouTube = YouTube.Default;
        }

        private void InitializeFileWatcher() {
            FileSystemDownloadWatcher = new FileSystemWatcher(TempPath) {
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size,
                Filter = "*.*"
            };

            FileSystemMusicWatcher = new FileSystemWatcher(MusicPath) {
                EnableRaisingEvents = true,
                Filter = "*.*",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size
            };
            FileSystemDownloadWatcher.Created += FileSystemDownloadWatcherOnCreated;
            FileSystemMusicWatcher.Created += FileSystemMusicWatcherOnCreated;
        }


        public async void Initialize() {
            if (!IsInternetAvailable()) {
                return;
            }

            await Task.Run(async () => {
                foreach (var droppedSong in ApplicationModel.DroppedSongs) {
                    var song = DownloadSongInformation(droppedSong);
                    if (song == null) {
                        song.Status = false;
                    }
                    else {
                        song.YouTubeUri = await SearchForSong(song.SongName, song.Artists?[0]);
                        if (string.IsNullOrEmpty(song.YouTubeUri)) {
                            song.Status = false;
                        }

                        Application.Current.Dispatcher.Invoke(delegate {
                            if (ApplicationModel.DownloadCollection.All(i => i.Id != song.Id)) {
                                ApplicationModel.DownloadCollection.Add(song);
                            }
                        });
                    }
                }  


            });
        }
        #endregion

        #region Get Song Information 

        public SongModel DownloadSongInformation(string id) {
            try {
                string loadedData;
                using (var webClient = new WebClient()) {
                    loadedData = webClient.DownloadString(SongInformationUri + PrepareId(id));
                }

                var desirializedInfo = JsonConvert.DeserializeObject<JObject>(loadedData);
                var songModel = new SongModel {
                    Id = PrepareId(id),
                    Artists = new List<string>(),
                    AlbumName = (string)desirializedInfo["album"]["name"],
                    SongName = (string)desirializedInfo["name"],
                    CoverImage = (string)desirializedInfo["album"]["images"][0]["url"],
                    Progress = 0,
                };

                foreach (var artist in desirializedInfo["artists"]) {
                    songModel.Artists.Add((string)artist["name"]);
                }
                return songModel;
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }
        #endregion

        #region Search YouTube 

        /// <summary>
        /// Searches YouTube for the Uri
        /// </summary>
        /// <param name="songName"></param>
        /// <param name="artist"></param>
        /// <returns></returns>
        public async Task<string> SearchForSong(string songName, string artist) {

            var searchListRequest = YouTubeService.Search.List(SearchInfo);
            searchListRequest.Q = songName; // Search Term
            searchListRequest.MaxResults = 50;

            var searchListResponse = await searchListRequest.ExecuteAsync();

            var matchingItems = searchListResponse.Items.Where(x => 
                x.Snippet.Title.Contains(songName) && x.Snippet.Title.Contains(artist)).ToList();

            if (matchingItems.Count == 0) {
                return string.Empty;
            }

            //Checks Content for VEVO
            if (matchingItems.Count > 1) {
                var vevoMatches = matchingItems.Where(x => x.Snippet.Title.Contains(Vevo)).ToList();
                if (vevoMatches.Count > 0) {
                    return YouTubeUri + vevoMatches[0].Id.VideoId;
                }
            }

            return YouTubeUri + matchingItems[0];
        }
        #endregion

        #region Download Songs

        private async void DownloadSongs() {

            if (!IsInternetAvailable()) {
                return;
            }

            await Task.Run(() => {
                foreach (var song in ApplicationModel.DownloadCollection) {
                    if (!song.Status) {
                        continue;
                    }

                    var video = YouTube.GetVideo(song.YouTubeUri);
                    if (video != null) {
                        File.WriteAllBytes(TempPath + video.FullName, video.GetBytes());
                    }
                }
            });

        }

        #endregion

        /// <summary>
        /// Notifys when File Converted and Done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fileSystemEventArgs"></param>
        private void FileSystemMusicWatcherOnCreated(object sender, FileSystemEventArgs fileSystemEventArgs) {

            var finishedSong =
                ApplicationModel.DownloadCollection.FirstOrDefault(x => x.SongName.Contains(fileSystemEventArgs.Name));

            if (finishedSong == null) {
                return;
            }

            ConvertService.KillFFmpegProcess();
            ApplicationModel.DownloadCollection.Remove(finishedSong);
            ApplicationModel.SongCollection.Add(finishedSong);
        }

        /// <summary>
        /// Notifys when Download is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fileSystemEventArgs"></param>
        private void FileSystemDownloadWatcherOnCreated(object sender, FileSystemEventArgs fileSystemEventArgs) {
            
        }

        #region Notify

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion 
    }
}
