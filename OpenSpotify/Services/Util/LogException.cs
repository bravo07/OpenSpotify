using System;
using System.IO;
using System.Text;
using static OpenSpotify.Services.Util.Utils;

namespace OpenSpotify.Services.Util {
    public class LogException {
        public LogException(Exception exception) {
            Exception = exception;
            Log();
        }

        public Exception Exception { get; set; }
        public StringBuilder StringBuilder { get; set; }

        private void Log() {
            StringBuilder = new StringBuilder();
            StringBuilder.Append($"Exception Found:\nType: {Exception.GetType().FullName}");
            StringBuilder.Append($"\nMessage: {Exception.Message}");
            StringBuilder.Append($"\nSource: {Exception.Source}");
            StringBuilder.Append($"\nStacktrace: {Exception.StackTrace}");

            using (var streamWriter = new StreamWriter(Path.Combine(LogPath, $"{Guid.NewGuid()}.txt"))) {
                streamWriter.Write(StringBuilder.ToString());
            }
        }
    }
}