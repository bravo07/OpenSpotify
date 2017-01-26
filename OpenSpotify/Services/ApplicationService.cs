using System;
using System.IO;
using Newtonsoft.Json;
using OpenSpotify.Models;
using static OpenSpotify.Services.Util.Utils;

namespace OpenSpotify.Services {

    public class ApplicationService {

        public static void InitializeApplicationDirectorys() {
            if (!Directory.Exists(ApplicationPath)) {
                Directory.CreateDirectory(ApplicationPath);
            }

            if (!Directory.Exists(MusicPath)) {
                Directory.CreateDirectory(MusicPath);
            }

            if (!Directory.Exists(TempPath)) {
                Directory.CreateDirectory(TempPath);
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
