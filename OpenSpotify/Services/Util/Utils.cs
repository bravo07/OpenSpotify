using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace OpenSpotify.Services.Util {

    public class Utils {

        #region Properties

        public static string ApplicationPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OpenSpotify");

        public static string MusicPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "OpenSpotify");

        public static string TempPath { get; set; } =
            Path.Combine(ApplicationPath, "Temp");

        public static string ApplicationDataPath = Path.Combine(ApplicationPath, "ApplicationModel.json");

        public static Uri SongInformationUri { get; set; } = new Uri("https://api.spotify.com/v1/tracks/");

        public static string YouTubeUri { get; set; } = "http://www.youtube.com/watch?v=";

        public static string SearchInfo => "snippet";

        public static string Vevo => "VEVO";

        public static string FFmpegCommand { get; set; } = "ffmpeg -i ";

        public static string FFmpegName => "ffmpeg";
        #endregion 

        #region WinAPI

        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        /// <summary>
        /// Checks for available Connection | more reliable then pinging Google
        /// </summary>
        /// <returns></returns>
        public static bool IsInternetAvailable() {
            int description;
            return InternetGetConnectedState(out description, 0);
        }
        #endregion

        #region Prepare Id

        /// <summary>
        /// Prepares id for the Song Information Download
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string PrepareId(string id) {
            return id.Substring(id.LastIndexOf("k/", StringComparison.Ordinal) + 2);
        }
        #endregion 
        public static string RemoveSpecialCharacters(string source) {
            return Regex.Replace(source, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
    }

    public static class StringExtensions {
        public static bool Contains(this string source, string toCheck, StringComparison comp) {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
