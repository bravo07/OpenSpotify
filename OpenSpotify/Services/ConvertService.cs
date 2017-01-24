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

        public void StartFFmpeg() {
            if (!File.Exists(ApplicationModel.Settings.FFmpegPath)) {
                MessageBox.Show("Catastrophic Failure",
                    "FFmpeg not Found!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            using (var ffmpegProcess = new Process()) {
                var processStartInfo = new ProcessStartInfo {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = ApplicationModel.Settings.FFmpegPath,
                    Arguments = $"-i {SongFileName} -b:a " +
                                $"{ApplicationModel.Settings.SelectedBitrate.Bitrate} -vn " +
                                $"{Path.Combine(MusicPath, Path.GetFileNameWithoutExtension(SongFileName))}" +
                                $"{ApplicationModel.Settings.SelectedFormat.Format}"
                };
                ffmpegProcess.StartInfo = processStartInfo;
                ffmpegProcess.Start();
            }
        }

        public void KillFFmpegProcess() {
            if (Process.GetProcessesByName(FFmpegName).Length > 0) {
                FFmpegProcess?.Kill();
            }
        }
    }
}
