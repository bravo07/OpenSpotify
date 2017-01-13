using System;
using System.IO;
using Newtonsoft.Json;
using OpenSpotify.Models;

namespace OpenSpotify.Services {

    public class ApplicationService {

        public static string ApplicationPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OpenSpotify");

        public static string MusicPath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "OpenSpotify");

        public static string ApplicationDataPath = Path.Combine(ApplicationPath, "ApplicationModel.json");

        public static void InitializeApplicationDirectorys() {
            if (!Directory.Exists(ApplicationPath)) {
                Directory.CreateDirectory(ApplicationPath);
            }

            if (!Directory.Exists(MusicPath)) {
                Directory.CreateDirectory(MusicPath);
            }
        }

        public static ApplicationModel LoadApplicationModel() {
            if (!File.Exists(ApplicationDataPath)) {
                return null;
            }

            using (var streamReader = new StreamReader(ApplicationDataPath)) {
                return JsonConvert.DeserializeObject<ApplicationModel>(streamReader.ReadToEnd());
            }
        }

        public static void SaveApplicationModel(ApplicationModel applicationModel) {
            using (var streamWriter = new StreamWriter(ApplicationDataPath)) {
                streamWriter.Write(JsonConvert.SerializeObject(applicationModel));
            }
        }
    }
}
