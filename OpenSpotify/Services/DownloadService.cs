using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenSpotify.Models;
using static OpenSpotify.Services.Util.Utils;

namespace OpenSpotify.Services {
    public class DownloadService {

        public static Uri SongInformationUri { get; set; } = new Uri("https://api.spotify.com/v1/tracks/");


        public static SongModel DownloadSongInformation(string id) {
            if (!IsInternetAvailable()) {
                return null;
            }

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
    }
}
