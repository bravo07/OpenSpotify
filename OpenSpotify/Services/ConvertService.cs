using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using OpenSpotify.Models;
using static OpenSpotify.Services.Util.Utils;
// ReSharper disable AssignNullToNotNullAttribute

namespace OpenSpotify.Services {

    public class ConvertService {

        public ConvertService(ApplicationModel applicationModel) {
            ApplicationModel = applicationModel;
        }

        public ApplicationModel ApplicationModel { get; set; }

        public Process FFmpegProcess { get; set; }

        public string SongFileName { get; set; }

        public async void StartFFmpeg() {
            if (!File.Exists(ApplicationModel.Settings.FFmpegPath)) {
                MessageBox.Show("Catastrophic Failure", 
                    "FFmpeg not Found!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            await Task.Run(() => {
                var processStartInfo = new ProcessStartInfo {
                    FileName = ApplicationModel.Settings.FFmpegPath,
                    Arguments = $"{FFmpegCommand} {SongFileName} -b:a " +
                            $"{ApplicationModel.Settings.SelectedBitrate.Bitrate} -vn " +
                            $"{Path.Combine(MusicPath, Path.GetFileNameWithoutExtension(SongFileName))}.mp3"
                };
                Process.Start(processStartInfo);
            });
            
        }

        public void KillFFmpegProcess() {
            if (Process.GetProcessesByName(FFmpegName).Length > 0) {
                FFmpegProcess?.Kill();
            }
        }
    }
}
