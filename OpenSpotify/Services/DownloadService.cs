using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSpotify.Models;
using VideoLibrary;
using static OpenSpotify.Services.Util.Utils;
using OpenSpotify.Services.Util;

namespace OpenSpotify.Services {

    public class DownloadService : INotifyPropertyChanged {
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

        public DateTime LastDownload { get; set; } = DateTime.MinValue;

        public DateTime LastMusic { get; set; } = DateTime.MinValue;
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
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size,
                Filter = "*.*",
            };
            FileSystemDownloadWatcher.Created += FileSystemDownloadWatcherOnCreated;
            FileSystemMusicWatcher.Created += FileSystemMusicWatcherOnCreated;
        }


        public async void Initialize() {
            if (!IsInternetAvailable()) {
                return;
            }

            foreach (var droppedSong in ApplicationModel.DroppedSongs) {
                var song = DownloadSongInformation(droppedSong);
                song.StatusValue = 30;
                if (song == null) {
                    song.Status = false;
                }
                else {
                    song.YouTubeUri = await SearchForSong(song.SongName, song.Artists?[0]);
                    if (string.IsNullOrEmpty(song.YouTubeUri)) {
                        song.Status = false;
                        return;
                    }

                    Application.Current.Dispatcher.Invoke(delegate {
                        if (ApplicationModel.DownloadCollection.All(i => i.Id != song.Id)) {
                            song.Status = true;
                            ApplicationModel.DownloadCollection.Add(song);
                        }
                    });
                }
            }

            if (ApplicationModel.DownloadCollection.Count > 0) {
                DownloadSongs();
            }
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
                    StatusValue = 60,
                    Progress = 0,
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
            searchListRequest.Q = $"{songName} {artist}"; // Search Term
            searchListRequest.MaxResults = 50;

            var searchListResponse = await searchListRequest.ExecuteAsync();

            var matchingItems = searchListResponse.Items.Where(x =>
                x.Snippet.Title.Contains(songName, StringComparison.OrdinalIgnoreCase) &&
                x.Snippet.Title.Contains(artist, StringComparison.OrdinalIgnoreCase)).ToList();

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

            return YouTubeUri + matchingItems[0].Id.VideoId;
        }
        #endregion

        #region Download Songs

        private void DownloadSongs() {

            if (!IsInternetAvailable()) {
                return;
            }

            foreach (var song in ApplicationModel.DownloadCollection) {
                if (!song.Status) {
                    continue;
                }

                song.StatusValue = 80;
                var video = YouTube.GetVideo(song.YouTubeUri);
                if (video != null) {
                    song.FileName = Path.GetFileNameWithoutExtension(RemoveSpecialCharacters(video.FullName.Replace(" ", string.Empty)));
                    File.WriteAllBytes(TempPath + "\\" + RemoveSpecialCharacters(video.FullName.Replace(" ", string.Empty)), video.GetBytes());
                }
            }
        }

        #endregion

        /// <summary>
        /// Notifys when File Converted and Done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fileSystemEventArgs"></param>
        private void FileSystemMusicWatcherOnCreated(object sender, FileSystemEventArgs fileSystemEventArgs) {

            var lastWriteTime = File.GetLastWriteTime(fileSystemEventArgs.FullPath);
            if (lastWriteTime.Subtract(LastMusic).Ticks > 0) {
                var finishedSong =
                    ApplicationModel.DownloadCollection.FirstOrDefault(
                        x => x.FileName.Contains(Path.GetFileNameWithoutExtension(
                            fileSystemEventArgs.Name), StringComparison.OrdinalIgnoreCase));
                if (finishedSong == null) {
                    return;
                }

                Application.Current.Dispatcher.Invoke(() => {
                    ConvertService.KillFFmpegProcess();
                    finishedSong.StatusValue = 100;
                    ApplicationModel.DownloadCollection.Remove(finishedSong);
                    ApplicationModel.SongCollection.Add(finishedSong);
                });
            }
        }

        /// <summary>
        /// Notifys when Download is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fileSystemEventArgs"></param>
        private void FileSystemDownloadWatcherOnCreated(object sender, FileSystemEventArgs fileSystemEventArgs) {

            var lastWriteTime = File.GetLastWriteTime(fileSystemEventArgs.FullPath);
            if (lastWriteTime.Subtract(LastDownload).Ticks > 0) {
                if (ConvertService == null) {
                    ConvertService = new ConvertService(ApplicationModel) {
                        SongFileName = fileSystemEventArgs.FullPath
                    };
                    ConvertService.StartFFmpeg();
                }
                else {
                    ConvertService.SongFileName = fileSystemEventArgs.FullPath;
                    ConvertService.StartFFmpeg();
                }
            }
        }

        #region Notify

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion 
    }
}
