using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using OpenSpotify.Models;

namespace OpenSpotify.Services.Util {
    public class Utils {

        #region Prepare Id

        /// <summary>
        ///     Prepares id for the Song Information Download
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string PrepareId(string id) {
            return id.Substring(id.LastIndexOf("k/", StringComparison.Ordinal) + 2);
        }

        #endregion

        #region Clear Temp

        public static void ClearTemp() {
            try {
                foreach(var file in Directory.GetFiles(TempPath)) if(File.Exists(file)) File.Delete(file);
            }
            catch(Exception) {
#if !DEBUG
                new LogException(ex);
#endif
            }
        }

        #endregion

        #region Set Status Image

        public static void SetStatus(SongModel song, Status status) {
            Application.Current.Dispatcher.Invoke(() => {
                switch(status) {
                    case Status.Downloading:
                        song.Status = "Downloading...";
                        song.StatusImage = StatusImageDownload;
                        break;
                    case Status.Converting:
                        song.Status = "Converting...";
                        song.StatusImage = StatusImageConvert;
                        break;
                    case Status.Done:
                        song.Status = "Done!";
                        song.StatusImage = StatusImageDone;
                        break;
                    case Status.Failed:
                        song.Status = "Failed!";
                        song.StatusImage = StatusImageFailed;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(status), status, null);
                }
            });
        }

        #endregion

        #region Remove Special Chars

        public static string RemoveSpecialCharacters(string source) {
            return Regex.Replace(source, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        #endregion

        #region Properties

        public static string ApplicationPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OpenSpotify");

        public static string MusicPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "OpenSpotify");

        public static string TempPath { get; set; } =
            Path.Combine(ApplicationPath, "Temp");

        public static string LogPath { get; set; } =
            Path.Combine(ApplicationPath, "Logs");

        public static string ApplicationDataPath = Path.Combine(ApplicationPath, "ApplicationModel.json");

        public static Uri SongInformationUri => new Uri("https://api.spotify.com/v1/tracks/");

        public static string YouTubeUri => "http://www.youtube.com/watch?v=";

        public static string SearchInfo => "snippet";

        public static string Vevo => "VEVO";

        public static string Audio => "Audio";

        public static string FFmpegCommand { get; set; } = "ffmpeg -i ";

        public static string FFmpegName => "ffmpeg";

        public static string LoadingSongInformation => "Loading Song Information";

        public static string FailedLoadingSongInformation => "Failed to load Song Information";

        public static string Downloading => "Downloading...";

        public static string NoInternet => "No Internet!";

        public static string NotReady => "No API Key or FFmpeg detected!";

        public static string Finished => "Finished.";

        public static string FailedYoutTubeUri => "Failed to load YouTube Uri.";

        public static string Converting => "Converting...";

        #endregion

        #region Images

        public static BitmapImage SoundImage100
            => new BitmapImage(new Uri("/Assets/PlayerSound100.png", UriKind.RelativeOrAbsolute));

        public static BitmapImage SoundImage50
            => new BitmapImage(new Uri("/Assets/PlayerSound50.png", UriKind.RelativeOrAbsolute));

        public static BitmapImage SoundImage10
            => new BitmapImage(new Uri("/Assets/PlayerSound10.png", UriKind.RelativeOrAbsolute));

        public static BitmapImage SoundImageOff
            => new BitmapImage(new Uri("/Assets/PlayerSoundOff.png", UriKind.RelativeOrAbsolute));

        public static BitmapImage StatusImageDownload
            => new BitmapImage(new Uri("/Assets/Download.png", UriKind.RelativeOrAbsolute));

        public static BitmapImage StatusImageConvert
            => new BitmapImage(new Uri("/Assets/Convert.png", UriKind.RelativeOrAbsolute));

        public static BitmapImage StatusImageFailed
            => new BitmapImage(new Uri("/Assets/Remove.png", UriKind.RelativeOrAbsolute));

        public static BitmapImage StatusImageDone
            => new BitmapImage(new Uri("/Assets/Play.png", UriKind.RelativeOrAbsolute));

        #endregion

        #region Check Internet

        /// <summary>
        ///     Checks for available Connection | more reliable then pinging Google
        /// </summary>
        /// <param name="description"></param>
        /// <param name="reservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable() {
            int description;
            return InternetGetConnectedState(out description, 0);
        }

        #endregion
    }

    public static class StringExtensions {
        public static bool Contains(this string source, string toCheck, StringComparison comp) {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}