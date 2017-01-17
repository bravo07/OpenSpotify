using System;
using System.IO;
using Newtonsoft.Json;
using OpenSpotify.Models;
using static OpenSpotify.Services.Util.Utils;

namespace OpenSpotify.Services {

    public class ApplicationService {

        /// <summary>
        /// Initializes needed Directories for the Application
        /// </summary>
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

        /// <summary>
        /// Desirializes Saved Application Settings 
        /// </summary>
        /// <returns></returns>
        public static ApplicationModel LoadApplicationModel() {
            if (!File.Exists(ApplicationDataPath)) {
                return null;
            }

            using (var streamReader = new StreamReader(ApplicationDataPath)) {
                return JsonConvert.DeserializeObject<ApplicationModel>(streamReader.ReadToEnd());
            }
        }

        /// <summary>
        /// Serializes Application Settings to AppData
        /// </summary>
        /// <param name="applicationModel"></param>
        public static void SaveApplicationModel(ApplicationModel applicationModel) {
            using (var streamWriter = new StreamWriter(ApplicationDataPath)) {
                streamWriter.Write(JsonConvert.SerializeObject(applicationModel));
            }
        }
    }
}
