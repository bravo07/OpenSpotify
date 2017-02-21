using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenSpotify.Models;
using OpenSpotify.Services.Util;
using static OpenSpotify.Services.Util.Utils;

namespace OpenSpotify.Services {
    public class ApplicationService {

        #region Initalization

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

        #endregion

        #region Load / Save Application

        public static ApplicationModel LoadApplicationModel() {
            try {
                if (!File.Exists(ApplicationDataPath)) {
                    return null;
                }

                using (var streamReader = new StreamReader(ApplicationDataPath)) {
                    var appModel = JsonConvert.DeserializeObject<ApplicationModel>(streamReader.ReadToEnd());
                    CheckSongs(ref appModel);
                    return appModel;
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

        #region CheckSongs

        private static void CheckSongs(ref ApplicationModel applicationModel) {
            for (var i = applicationModel.SongCollection.Count - 1; i >= 0; i--) {
                if (!File.Exists(applicationModel.SongCollection[i].FullPath)) {
                    applicationModel.SongCollection.RemoveAt(i);
                }
            }
        }

        #endregion
    }
}