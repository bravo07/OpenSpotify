using System.IO;
using Newtonsoft.Json;
using OpenSpotify.Models;
using OpenSpotify.Services.Util;
using static OpenSpotify.Services.Util.Utils;

namespace OpenSpotify.Services {
    public class ApplicationService {
        public static void InitializeApplicationDirectorys() {
            try {
                if (!Directory.Exists(ApplicationPath)) {
                    Directory.CreateDirectory(ApplicationPath);
                }

                if (!Directory.Exists(MusicPath)) {
                    Directory.CreateDirectory(MusicPath);
                }

                if (!Directory.Exists(TempPath)) {
                    Directory.CreateDirectory(TempPath);
                }

                if (!Directory.Exists(LogPath)) {
                    Directory.CreateDirectory(LogPath);
                }
            }
            catch (System.Exception ex) {
                new LogException(ex);
            }
        }

        #region Load / Save Application

        public static ApplicationModel LoadApplicationModel() {
            try {
                if (!File.Exists(ApplicationDataPath)) {
                    return null;
                }

                using (var streamReader = new StreamReader(ApplicationDataPath)) {
                    return JsonConvert.DeserializeObject<ApplicationModel>(streamReader.ReadToEnd());
                }
            }
            catch (System.Exception ex) {
                new LogException(ex);
            }
            return null;
        }

        public static void SaveApplicationModel(ApplicationModel applicationModel) {
            try {
                using (var streamWriter = new StreamWriter(ApplicationDataPath)) {
                    streamWriter.Write(JsonConvert.SerializeObject(applicationModel));
                }
            }
            catch (System.Exception ex) {
                new LogException(ex);
            }
        }

        #endregion
    }
}