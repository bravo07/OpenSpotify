using System;
using System.Runtime.InteropServices;

namespace OpenSpotify.Services.Util {

    public class Utils {

        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable() {
            int description;
            return InternetGetConnectedState(out description, 0);
        }

        public static string PrepareId(string id) {
            return id.Substring(id.LastIndexOf("k/", StringComparison.Ordinal) + 2);
        }

    }
}
